// OrderController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkinCare_Service.IService;
using SkinCare_Data.Data;
using SkinCare_Data.DTO.Order;
using SkinCare_Data.IRepositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using SkinCare_Data.DTO.Orderdetai;

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
        [HttpGet("staff/orders")]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult<List<OrderStaffDTO>>> GetAllOrdersForStaff()
        {
            try
            {
                _logger.LogInformation("Staff is retrieving all orders.");

                var orders = await _orderService.GetAllOrdersForStaffAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders: {ErrorMessage}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("staff/order-details/{orderId}")]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult<OrderStaffDTO>> GetOrderDetailsForStaff(string orderId)
        {
            try
            {
                _logger.LogInformation("Staff is retrieving details for order: {OrderId}", orderId);

                var order = await _orderService.GetOrderDetailsForStaffAsync(orderId);
                if (order == null)
                {
                    return NotFound(new { message = "Order not found." });
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order details for {OrderId}: {ErrorMessage}", orderId, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("staff/create-order")]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult<OrderStaffDTO>> CreateOrderForStaff([FromBody] CreateOrderForStaffDTO createOrderDto)
        {
            try
            {
                _logger.LogInformation("Staff is creating a new order.");

                var newOrder = await _orderService.CreateOrderForStaffAsync(createOrderDto);
                return Ok(newOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order: {ErrorMessage}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPut("staff/update-order-status")]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult<OrderStaffDTO>> UpdateOrderStatus([FromBody] UpdateOrderStatusDTO updateOrderDto)
        {
            try
            {
                _logger.LogInformation("Staff is updating order status.");

                var updatedOrder = await _orderService.UpdateOrderStatusAsync(updateOrderDto);
                return Ok(updatedOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order status: {ErrorMessage}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("staff/delete-order/{orderId}")]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult<OrderStaffDTO>> DeleteOrder(string orderId)
        {
            try
            {
                _logger.LogInformation("Staff is deleting order {OrderId}", orderId);

                var deletedOrder = await _orderService.DeleteOrderAsync(orderId);
                return Ok(new { message = "Order deleted successfully", deletedOrder });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order {OrderId}: {ErrorMessage}", orderId, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }
    }


    public class UpdateQuantityDto
    {
        public int QuantityChange { get; set; } 
    }
}