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
            // Giữ nguyên logic hiện tại
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
    }
}