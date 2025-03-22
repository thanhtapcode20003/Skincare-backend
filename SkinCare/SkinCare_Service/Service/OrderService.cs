// SkinCare_Service/OrderService.cs
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
using SkinCare.Utilities; // Thêm namespace cho VNPayHelper

namespace SkinCare_Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<OrderService> _logger;
        private readonly VNPayHelper _vnpayHelper; // Thêm VNPayHelper

        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            ILogger<OrderService> logger,
            VNPayHelper vnpayHelper) // Thêm vào constructor
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _logger = logger;
            _vnpayHelper = vnpayHelper;
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

        // Thêm phương thức tạo URL thanh toán VNPay
        public async Task<string> CreateVNPayPaymentUrlAsync(string userId, string orderId, string ipAddress)
        {
            try
            {
                _logger.LogInformation("Creating VNPay Sandbox payment URL for order {OrderId} for user {UserId}", orderId, userId);

                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null || order.UserId != userId)
                {
                    throw new Exception("Order not found or you are not authorized!");
                }

                if (order.OrderStatus != "Cart")
                {
                    throw new Exception("Order must be in Cart status to proceed with payment!");
                }

                string paymentUrl = _vnpayHelper.CreatePaymentUrl(orderId, order.TotalAmount, ipAddress);
                return paymentUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating VNPay Sandbox payment URL for order {OrderId}: {ErrorMessage}", orderId, ex.Message);
                throw;
            }
        }

        // Thêm phương thức xử lý callback từ VNPay
        public async Task<bool> HandleVNPayCallbackAsync(Dictionary<string, string> vnpayData)
        {
            try
            {
                _logger.LogInformation("Handling VNPay Sandbox callback for transaction {TransactionId}", vnpayData["vnp_TxnRef"]);

                bool isValid = _vnpayHelper.VerifyCallback(vnpayData);
                if (!isValid)
                {
                    _logger.LogWarning("Invalid VNPay Sandbox callback signature for transaction {TransactionId}", vnpayData["vnp_TxnRef"]);
                    return false;
                }

                string orderId = vnpayData["vnp_TxnRef"];
                string responseCode = vnpayData["vnp_ResponseCode"];
                string transactionStatus = vnpayData["vnp_TransactionStatus"];

                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                {
                    throw new Exception("Order not found!");
                }

                if (responseCode == "00" && transactionStatus == "00") // Thanh toán thành công
                {
                    order.OrderStatus = "Confirmed";
                    await _orderRepository.UpdateOrderAsync(order);
                    await _orderRepository.SaveChangesAsync();
                    _logger.LogInformation("Sandbox payment successful for order {OrderId}", orderId);
                    return true;
                }
                else
                {
                    _logger.LogWarning("Sandbox payment failed for order {OrderId}. ResponseCode: {ResponseCode}", orderId, responseCode);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling VNPay Sandbox callback: {ErrorMessage}", ex.Message);
                throw;
            }
        }
    }
}