// OrderController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkinCare_Service.IService;
using SkinCare_Data.Data;
using SkinCare_Data.DTO.Order;
using SkinCare_Data.IRepositories;
using System.Threading.Tasks;
using System.Collections.Generic;

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
        public async Task<ActionResult<List<OrderSummaryDto>>> GetMyOrders() // Cập nhật kiểu trả về
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
    }
}