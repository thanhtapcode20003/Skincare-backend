// OrderService.cs
using Microsoft.Extensions.Logging;
using SkinCare_Data.Data;
using SkinCare_Data.IRepositories;
using SkinCare_Service.IService;
using SkinCare_Data.DTO.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SkinCare_Data.DTO.Orderdetai;

namespace SkinCare_Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<Order> AddToCartAsync(string userId, AddToCartDto addToCartDto)
        {
            try
            {
                _logger.LogInformation("Adding product {ProductId} to cart for user {UserId}", addToCartDto.ProductId, userId);

                var product = await _productRepository.GetByIdAsync(addToCartDto.ProductId);
                if (product == null)
                {
                    throw new Exception("Product not found!");
                }

                if (product.Quantity < addToCartDto.Quantity)
                {
                    throw new Exception($"Insufficient stock for product {product.ProductName}. Available: {product.Quantity}");
                }

                using (var transaction = await _orderRepository.GetContext().Database.BeginTransactionAsync())
                {
                    try
                    {
                        var cart = await _orderRepository.GetCartByUserIdAsync(userId);

                        if (cart == null)
                        {
                            string newOrderId;
                            int orderCount = await _orderRepository.GetOrderCountAsync() + 1;
                            newOrderId = $"O{orderCount:D3}";
                            while (await _orderRepository.GetByIdAsync(newOrderId) != null)
                            {
                                orderCount++;
                                newOrderId = $"O{orderCount:D3}";
                            }

                            cart = new Order
                            {
                                OrderId = newOrderId,
                                UserId = userId,
                                OrderStatus = "Cart",
                                TotalAmount = 0,
                                CreateAt = DateTime.UtcNow,
                                OrderDetails = new List<OrderDetail>()
                            };
                            await _orderRepository.AddOrderAsync(cart);
                            await _orderRepository.SaveChangesAsync();
                        }

                        var existingOrderDetail = cart.OrderDetails
                            .FirstOrDefault(od => od.ProductId == addToCartDto.ProductId);

                        if (existingOrderDetail != null)
                        {
                            int newQuantity = existingOrderDetail.Quantity + addToCartDto.Quantity;
                            if (product.Quantity < newQuantity)
                            {
                                throw new Exception($"Insufficient stock for product {product.ProductName}. Available: {product.Quantity}");
                            }
                            existingOrderDetail.Quantity = newQuantity;
                            existingOrderDetail.Price = product.Price;
                        }
                        else
                        {
                            string newOrderDetailId;
                            int orderDetailCount = await _orderRepository.GetOrderDetailCountAsync() + 1;
                            newOrderDetailId = $"OD{orderDetailCount:D3}";
                            while (await _orderRepository.GetContext().OrderDetails.FirstOrDefaultAsync(od => od.OrderDetailId == newOrderDetailId) != null)
                            {
                                orderDetailCount++;
                                newOrderDetailId = $"OD{orderDetailCount:D3}";
                            }

                            var orderDetail = new OrderDetail
                            {
                                OrderDetailId = newOrderDetailId,
                                OrderId = cart.OrderId,
                                ProductId = addToCartDto.ProductId,
                                Price = product.Price,
                                Quantity = addToCartDto.Quantity
                            };
                            cart.OrderDetails.Add(orderDetail);
                            await _orderRepository.AddOrderDetailAsync(orderDetail);
                        }

                        cart.TotalAmount = (int)cart.OrderDetails.Sum(od => od.Price * od.Quantity);
                        product.Quantity -= addToCartDto.Quantity;
                        await _productRepository.UpdateAsync(product);

                        await _orderRepository.UpdateOrderAsync(cart);
                        await _orderRepository.SaveChangesAsync();

                        await transaction.CommitAsync();
                        return cart;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        _logger.LogError(ex, "Error adding product to cart for user {UserId}: {ErrorMessage}", userId, ex.Message);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product to cart for user {UserId}: {ErrorMessage}", userId, ex.Message);
                throw;
            }
        }

        public async Task<List<OrderSummaryDto>> GetMyOrdersAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Retrieving orders for user {UserId}", userId);
                var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
                var orderSummaries = orders.Select(o => new OrderSummaryDto
                {
                    OrderId = o.OrderId,
                    OrderStatus = o.OrderStatus,
                    TotalAmount = o.TotalAmount,
                    CreateAt = o.CreateAt
                }).ToList();

                if (orderSummaries == null || !orderSummaries.Any())
                {
                    _logger.LogInformation("No orders found for user {UserId}", userId);
                }
                return orderSummaries;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders for user {UserId}: {ErrorMessage}", userId, ex.Message);
                throw;
            }
        }

        public async Task<List<OrderDetailResponseDto>> GetMyOrderDetailsAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Retrieving order details for user {UserId}", userId);
                var orderDetails = await _orderRepository.GetOrderDetailsByUserIdAsync(userId);
                if (orderDetails == null || !orderDetails.Any())
                {
                    _logger.LogInformation("No order details found for user {UserId}", userId);
                    return new List<OrderDetailResponseDto>();
                }

                var response = orderDetails.Select(od => new OrderDetailResponseDto
                {
                    OrderDetailId = od.OrderDetailId,
                    OrderId = od.OrderId,
                    ProductId = od.ProductId,
                    ProductName = od.Product.ProductName,
                    Price = od.Price,
                    Quantity = od.Quantity
                }).ToList();

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order details for user {UserId}: {ErrorMessage}", userId, ex.Message);
                throw;
            }
        }

        public async Task<OrderDetailResponseDto> UpdateOrderDetailQuantityAsync(string userId, string orderDetailId, int quantityChange)
        {
            try
            {
                _logger.LogInformation("Updating quantity for order detail {OrderDetailId} for user {UserId}", orderDetailId, userId);
                var orderDetail = await _orderRepository.GetOrderDetailByIdAsync(orderDetailId);
                if (orderDetail == null)
                {
                    throw new Exception("Order detail not found!");
                }

                var order = await _orderRepository.GetByIdAsync(orderDetail.OrderId);
                if (order == null || order.UserId != userId)
                {
                    throw new Exception("Order not found or you are not authorized!");
                }

                var product = await _productRepository.GetByIdAsync(orderDetail.ProductId);
                if (product == null)
                {
                    throw new Exception("Product not found!");
                }

                int newQuantity = orderDetail.Quantity + quantityChange;
                if (newQuantity <= 0)
                {
                    throw new Exception("Quantity must be greater than 0!");
                }

                if (order.OrderStatus == "Cart" && quantityChange > 0 && product.Quantity < quantityChange)
                {
                    throw new Exception($"Insufficient stock for product {product.ProductName}. Available: {product.Quantity}");
                }

                orderDetail.Quantity = newQuantity;
                if (order.OrderStatus == "Cart")
                {
                    if (quantityChange > 0)
                    {
                        product.Quantity -= quantityChange;
                    }
                    else if (quantityChange < 0)
                    {
                        product.Quantity += Math.Abs(quantityChange);
                    }
                    await _productRepository.UpdateAsync(product);
                }

                order.TotalAmount = (int)order.OrderDetails.Sum(od => od.Price * od.Quantity);
                await _orderRepository.UpdateOrderDetailAsync(orderDetail);
                await _orderRepository.UpdateOrderAsync(order);
                await _orderRepository.SaveChangesAsync();

                return new OrderDetailResponseDto
                {
                    OrderDetailId = orderDetail.OrderDetailId,
                    OrderId = orderDetail.OrderId,
                    ProductId = orderDetail.ProductId,
                    ProductName = product.ProductName,
                    Price = orderDetail.Price,
                    Quantity = orderDetail.Quantity
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating quantity for order detail {OrderDetailId} for user {UserId}: {ErrorMessage}", orderDetailId, userId, ex.Message);
                throw;
            }
        }

        public async Task<List<OrderDetailResponseDto>> DeleteOrderDetailAsync(string userId, string orderDetailId)
        {
            try
            {
                _logger.LogInformation("Deleting order detail {OrderDetailId} for user {UserId}", orderDetailId, userId);
                var orderDetail = await _orderRepository.GetOrderDetailByIdAsync(orderDetailId);
                if (orderDetail == null)
                {
                    throw new Exception("Order detail not found!");
                }

                var order = await _orderRepository.GetByIdAsync(orderDetail.OrderId);
                if (order == null || order.UserId != userId)
                {
                    throw new Exception("Order not found or you are not authorized!");
                }

                var product = await _productRepository.GetByIdAsync(orderDetail.ProductId);
                if (product == null)
                {
                    throw new Exception("Product not found!");
                }

                if (order.OrderStatus == "Cart")
                {
                    product.Quantity += orderDetail.Quantity;
                    await _productRepository.UpdateAsync(product);
                }

                await _orderRepository.DeleteOrderDetailAsync(orderDetailId);
                order.TotalAmount = (int)order.OrderDetails.Sum(od => od.Price * od.Quantity);
                await _orderRepository.UpdateOrderAsync(order);
                await _orderRepository.SaveChangesAsync();

                var remainingOrderDetails = await _orderRepository.GetOrderDetailsByUserIdAsync(userId);
                var response = remainingOrderDetails.Select(od => new OrderDetailResponseDto
                {
                    OrderDetailId = od.OrderDetailId,
                    OrderId = od.OrderId,
                    ProductId = od.ProductId,
                    ProductName = od.Product.ProductName,
                    Price = od.Price,
                    Quantity = od.Quantity
                }).ToList();

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order detail {OrderDetailId} for user {UserId}: {ErrorMessage}", orderDetailId, userId, ex.Message);
                throw;
            }
        }
        public async Task<List<OrderStaffDTO>> GetAllOrdersForStaffAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all orders for staff.");

                var orders = await _orderRepository.GetAllOrdersAsync();

                var orderDtos = orders.Select(o => new OrderStaffDTO
                {
                    OrderId = o.OrderId,
                    UserId = o.UserId,
                    OrderStatus = o.OrderStatus,
                    TotalAmount = o.TotalAmount,
                    CreateAt = o.CreateAt,
                    OrderDetails = o.OrderDetails.Select(od => new OrderDetailDTO
                    {
                        ProductId = od.ProductId,
                        ProductName = od.Product.ProductName,
                        Price = od.Price,
                        Quantity = od.Quantity
                    }).ToList()
                }).ToList();

                return orderDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching orders for staff.");
                throw;
            }
        }
        public async Task<OrderStaffDTO> GetOrderDetailsForStaffAsync(string orderId)
        {
            try
            {
                _logger.LogInformation("Retrieving order details for Order ID: {OrderId}", orderId);

                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                {
                    _logger.LogWarning("Order with ID {OrderId} not found", orderId);
                    return null;
                }

                // Chuyển đổi sang DTO để hiển thị cho nhân viên
                var orderDto = new OrderStaffDTO
                {
                    OrderId = order.OrderId,
                    UserId = order.UserId,
                    OrderStatus = order.OrderStatus,
                    TotalAmount = order.TotalAmount,
                    CreateAt = order.CreateAt,
                    OrderDetails = order.OrderDetails.Select(od => new OrderDetailDTO
                    {
                        OrderDetailId = od.OrderDetailId,
                        ProductId = od.ProductId,
                        Price = od.Price,
                        Quantity = od.Quantity
                    }).ToList()
                };

                return orderDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order details for Order ID: {OrderId}", orderId);
                throw;
            }
        }

    }
}