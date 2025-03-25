using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkinCare_Data.DTO.RatingFeedBack;
using SkinCare_Service.IService;
using SkinCare_Service.Service.Iservice;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkinCare_API.Controllers
{
    [Route("api/ratings-feedback")]
    [ApiController]
    public class RatingsFeedbackController : ControllerBase
    {
        private readonly IRatingsFeedbackService _ratingsFeedbackService;
        private readonly ILogger<RatingsFeedbackController> _logger;

        public RatingsFeedbackController(
            IRatingsFeedbackService ratingsFeedbackService,
            ILogger<RatingsFeedbackController> logger)
        {
            _ratingsFeedbackService = ratingsFeedbackService;
            _logger = logger;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<RatingsFeedbackResponseDto>> CreateRatingsFeedback([FromBody] CreateRatingsFeedbackDto dto)
        {
            try
            {
                var userId = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var result = await _ratingsFeedbackService.CreateRatingsFeedbackAsync(userId, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating ratings feedback: {ErrorMessage}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{feedbackId}")]
        public async Task<ActionResult<RatingsFeedbackResponseDto>> GetRatingsFeedbackById(string feedbackId)
        {
            try
            {
                var result = await _ratingsFeedbackService.GetRatingsFeedbackByIdAsync(feedbackId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ratings feedback with ID {FeedbackId}: {ErrorMessage}", feedbackId, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<List<RatingsFeedbackResponseDto>>> GetRatingsFeedbackByProductId(string productId)
        {
            try
            {
                var result = await _ratingsFeedbackService.GetRatingsFeedbackByProductIdAsync(productId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ratings feedback for product {ProductId}: {ErrorMessage}", productId, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<List<RatingsFeedbackResponseDto>>> GetRatingsFeedbackByUserId(string userId)
        {
            try
            {
                var currentUserId = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                if (userId != currentUserId)
                {
                    return Unauthorized(new { message = "You are not authorized to view feedback of another user" });
                }

                var result = await _ratingsFeedbackService.GetRatingsFeedbackByUserIdAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ratings feedback for user {UserId}: {ErrorMessage}", userId, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{feedbackId}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<RatingsFeedbackResponseDto>> UpdateRatingsFeedback(string feedbackId, [FromBody] UpdateRatingsFeedbackDto dto)
        {
            try
            {
                var userId = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var feedback = await _ratingsFeedbackService.GetRatingsFeedbackByIdAsync(feedbackId);
                if (feedback.UserId != userId)
                {
                    return Unauthorized(new { message = "You are not authorized to update this feedback" });
                }

                var result = await _ratingsFeedbackService.UpdateRatingsFeedbackAsync(feedbackId, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ratings feedback with ID {FeedbackId}: {ErrorMessage}", feedbackId, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{feedbackId}")]
        [Authorize(Roles = "Customer, Manager")]
        public async Task<ActionResult> DeleteRatingsFeedback(string feedbackId)
        {
            try
            {
                var userId = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var feedback = await _ratingsFeedbackService.GetRatingsFeedbackByIdAsync(feedbackId);
                var userRole = User.FindFirst("Role")?.Value;

                if (feedback.UserId != userId && userRole != "Manager")
                {
                    return Unauthorized(new { message = "You are not authorized to delete this feedback" });
                }

                await _ratingsFeedbackService.DeleteRatingsFeedbackAsync(feedbackId);
                return Ok(new { message = "Ratings feedback deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ratings feedback with ID {FeedbackId}: {ErrorMessage}", feedbackId, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}