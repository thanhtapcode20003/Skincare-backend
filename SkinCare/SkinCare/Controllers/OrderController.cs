// SkinCare_API/Controllers/OrderController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkinCare_Service.IService;
using SkinCare_Data.Data;
using SkinCare_Data.DTO.Order;
using SkinCare_Data.IRepositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using SkinCare_Data.DTO.Orderdetai;
using SkinCare_Data.Repositories;

namespace SkinCare.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IAuthRepository _authRepository;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, IAuthRepository authRepository, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _authRepository = authRepository;
            _logger = logger;
        }

        [HttpPost("add-to-cart")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<Order>> AddToCart([FromBody] AddToCartDto addToCartDto)
        {
            try
            {
                _logger.LogInformation("Adding product {ProductId} to cart", addToCartDto.ProductId);

                var userId = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var user = await _authRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return BadRequest(new { message = "User not found" });
                }

                var cart = await _orderService.AddToCartAsync(userId, addToCartDto);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product to cart: {ErrorMessage}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("my-orders")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<List<OrderSummaryDto>>> GetMyOrders()
        {
            try
            {
                _logger.LogInformation("Retrieving orders for user");

                var userId = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var user = await _authRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return BadRequest(new { message = "User not found" });
                }

                var orders = await _orderService.GetMyOrdersAsync(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders: {ErrorMessage}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("my-order-details")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<List<OrderDetailResponseDto>>> GetMyOrderDetails()
        {
            try
            {
                _logger.LogInformation("Retrieving order details for user");

                var userId = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var user = await _authRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return BadRequest(new { message = "User not found" });
                }

                var orderDetails = await _orderService.GetMyOrderDetailsAsync(userId);
                return Ok(orderDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order details: {ErrorMessage}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{orderDetailId}/quantity")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<OrderDetailResponseDto>> UpdateOrderDetailQuantity(string orderDetailId, [FromBody] UpdateQuantityDto updateQuantityDto)
        {
            try
            {
                _logger.LogInformation("Updating quantity for order detail {OrderDetailId}", orderDetailId);

                var userId = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var user = await _authRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return BadRequest(new { message = "User not found" });
                }

                var updatedOrderDetail = await _orderService.UpdateOrderDetailQuantityAsync(userId, orderDetailId, updateQuantityDto.QuantityChange);
                return Ok(updatedOrderDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating quantity for order detail {OrderDetailId}: {ErrorMessage}", orderDetailId, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{orderDetailId}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<List<OrderDetailResponseDto>>> DeleteOrderDetail(string orderDetailId)
        {
            try
            {
                _logger.LogInformation("Deleting order detail {OrderDetailId}", orderDetailId);

                var userId = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var user = await _authRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return BadRequest(new { message = "User not found" });
                }

                var remainingOrderDetails = await _orderService.DeleteOrderDetailAsync(userId, orderDetailId);
                return Ok(remainingOrderDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order detail {OrderDetailId}: {ErrorMessage}", orderDetailId, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{orderId}/create-cod-payment")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult> CreateCODPayment(string orderId)
        {
            try
            {
                _logger.LogInformation("Processing COD payment for order {OrderId}", orderId);

                var userId = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var user = await _authRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return BadRequest(new { message = "User not found" });
                }

                await _orderService.HandleCODPaymentAsync(userId, orderId);
                return Ok(new { message = "COD payment processed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing COD payment for order {OrderId}: {ErrorMessage}", orderId, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{orderId}/create-vnpay-payment")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<string>> CreateVNPayPayment(string orderId)
        {
            try
            {
                _logger.LogInformation("Creating VNPay Sandbox payment for order {OrderId}", orderId);

                var userId = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var user = await _authRepository.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return BadRequest(new { message = "User not found" });
                }

                string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                var paymentUrl = await _orderService.CreateVNPayPaymentUrlAsync(userId, orderId, ipAddress);
                return Ok(new { paymentUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating VNPay Sandbox payment for order {OrderId}: {ErrorMessage}", orderId, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }


        // Thêm endpoint để xử lý callback từ VNPay
        [HttpGet("vnpay-callback")]
        public async Task<ActionResult> VNPayCallback()
        {
            try
            {
                _logger.LogInformation("Received VNPay Sandbox callback");

                // Extract query parameters from the VNPAY callback
                var vnpayData = Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString());
                bool isSuccess = await _orderService.HandleVNPayCallbackAsync(vnpayData);

                // Extract orderId and amount from vnpayData
                string orderId = vnpayData["vnp_TxnRef"];
                decimal amount = decimal.Parse(vnpayData["vnp_Amount"]) / 100; // Amount in VND (VNPAY sends it as amount * 100)

                // Construct the redirect URL for the frontend
                string status = isSuccess ? "success" : "failed";
                string redirectUrl = $"http://localhost:5173/payment/result?status={status}&amount={amount}&orderId={orderId}";

                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling VNPay Sandbox callback: {ErrorMessage}", ex.Message);

                // On error, redirect to the frontend with a failed status
                string redirectUrl = $"http://localhost:5173/payment/result?status=failed&amount=0&orderId=unknown";
                return Redirect(redirectUrl);
            }
        }

        public class UpdateQuantityDto
        {
            public int QuantityChange { get; set; }
        }
    }
}