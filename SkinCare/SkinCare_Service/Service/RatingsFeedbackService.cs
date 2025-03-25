using Microsoft.Extensions.Logging;
using SkinCare_Data.Data;
using SkinCare_Data.DTO.RatingFeedBack;
using SkinCare_Data.IRepositories;
using SkinCare_Data.Repositories.Irepositories;
using SkinCare_Service.IService;
using SkinCare_Service.Service.Iservice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkinCare_Service
{
    public class RatingsFeedbackService : IRatingsFeedbackService
    {
        private readonly IRatingsFeedbackRepository _ratingsFeedbackRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<RatingsFeedbackService> _logger;

        public RatingsFeedbackService(
            IRatingsFeedbackRepository ratingsFeedbackRepository,
            IProductRepository productRepository,
            IUserRepository userRepository,
            ILogger<RatingsFeedbackService> logger)
        {
            _ratingsFeedbackRepository = ratingsFeedbackRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<RatingsFeedbackResponseDto> CreateRatingsFeedbackAsync(string userId, CreateRatingsFeedbackDto dto)
        {
            try
            {
                _logger.LogInformation("Creating ratings feedback for user {UserId} and product {ProductId}", userId, dto.ProductId);

                // Kiểm tra user và product có tồn tại không
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found!");
                }

                var product = await _productRepository.GetByIdAsync(dto.ProductId);
                if (product == null)
                {
                    throw new Exception("Product not found!");
                }

                // Kiểm tra rating hợp lệ (1-5)
                if (dto.Rating < 1 || dto.Rating > 5)
                {
                    throw new Exception("Rating must be between 1 and 5!");
                }

                // Tạo feedback mới
                var feedback = new RatingsFeedback
                {
                    FeedbackId = Guid.NewGuid().ToString(),
                    UserId = userId, // Lấy từ tham số userId
                    ProductId = dto.ProductId,
                    Rating = dto.Rating,
                    Comment = dto.Comment,
                    CreateAt = DateTime.UtcNow
                };

                await _ratingsFeedbackRepository.AddAsync(feedback);
                await _ratingsFeedbackRepository.SaveChangesAsync();

                return new RatingsFeedbackResponseDto
                {
                    FeedbackId = feedback.FeedbackId,
                    UserId = feedback.UserId,
                    UserName = user.UserName,
                    ProductId = feedback.ProductId,
                    ProductName = product.ProductName,
                    Rating = feedback.Rating,
                    Comment = feedback.Comment,
                    CreateAt = feedback.CreateAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating ratings feedback for user {UserId}: {ErrorMessage}", userId, ex.Message);
                throw;
            }
        }

        public async Task<RatingsFeedbackResponseDto> GetRatingsFeedbackByIdAsync(string feedbackId)
        {
            try
            {
                _logger.LogInformation("Retrieving ratings feedback with ID {FeedbackId}", feedbackId);

                var feedback = await _ratingsFeedbackRepository.GetByIdAsync(feedbackId);
                if (feedback == null)
                {
                    throw new Exception("Ratings feedback not found!");
                }

                return new RatingsFeedbackResponseDto
                {
                    FeedbackId = feedback.FeedbackId,
                    UserId = feedback.UserId,
                    UserName = feedback.User.UserName,
                    ProductId = feedback.ProductId,
                    ProductName = feedback.Product.ProductName,
                    Rating = feedback.Rating,
                    Comment = feedback.Comment,
                    CreateAt = feedback.CreateAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ratings feedback with ID {FeedbackId}: {ErrorMessage}", feedbackId, ex.Message);
                throw;
            }
        }

        public async Task<List<RatingsFeedbackResponseDto>> GetRatingsFeedbackByProductIdAsync(string productId)
        {
            try
            {
                _logger.LogInformation("Retrieving ratings feedback for product {ProductId}", productId);

                var feedbacks = await _ratingsFeedbackRepository.GetByProductIdAsync(productId);
                return feedbacks.Select(f => new RatingsFeedbackResponseDto
                {
                    FeedbackId = f.FeedbackId,
                    UserId = f.UserId,
                    UserName = f.User.UserName,
                    ProductId = f.ProductId,
                    ProductName = f.Product.ProductName,
                    Rating = f.Rating,
                    Comment = f.Comment,
                    CreateAt = f.CreateAt
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ratings feedback for product {ProductId}: {ErrorMessage}", productId, ex.Message);
                throw;
            }
        }

        public async Task<List<RatingsFeedbackResponseDto>> GetRatingsFeedbackByUserIdAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Retrieving ratings feedback for user {UserId}", userId);

                var feedbacks = await _ratingsFeedbackRepository.GetByUserIdAsync(userId);
                return feedbacks.Select(f => new RatingsFeedbackResponseDto
                {
                    FeedbackId = f.FeedbackId,
                    UserId = f.UserId,
                    UserName = f.User.UserName,
                    ProductId = f.ProductId,
                    ProductName = f.Product.ProductName,
                    Rating = f.Rating,
                    Comment = f.Comment,
                    CreateAt = f.CreateAt
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ratings feedback for user {UserId}: {ErrorMessage}", userId, ex.Message);
                throw;
            }
        }

        public async Task<RatingsFeedbackResponseDto> UpdateRatingsFeedbackAsync(string feedbackId, UpdateRatingsFeedbackDto dto)
        {
            try
            {
                _logger.LogInformation("Updating ratings feedback with ID {FeedbackId}", feedbackId);

                var feedback = await _ratingsFeedbackRepository.GetByIdAsync(feedbackId);
                if (feedback == null)
                {
                    throw new Exception("Ratings feedback not found!");
                }

                // Kiểm tra rating hợp lệ (1-5)
                if (dto.Rating < 1 || dto.Rating > 5)
                {
                    throw new Exception("Rating must be between 1 and 5!");
                }

                feedback.Rating = dto.Rating;
                feedback.Comment = dto.Comment;

                await _ratingsFeedbackRepository.UpdateAsync(feedback);
                await _ratingsFeedbackRepository.SaveChangesAsync();

                return new RatingsFeedbackResponseDto
                {
                    FeedbackId = feedback.FeedbackId,
                    UserId = feedback.UserId,
                    UserName = feedback.User.UserName,
                    ProductId = feedback.ProductId,
                    ProductName = feedback.Product.ProductName,
                    Rating = feedback.Rating,
                    Comment = feedback.Comment,
                    CreateAt = feedback.CreateAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ratings feedback with ID {FeedbackId}: {ErrorMessage}", feedbackId, ex.Message);
                throw;
            }
        }

        public async Task DeleteRatingsFeedbackAsync(string feedbackId)
        {
            try
            {
                _logger.LogInformation("Deleting ratings feedback with ID {FeedbackId}", feedbackId);

                var feedback = await _ratingsFeedbackRepository.GetByIdAsync(feedbackId);
                if (feedback == null)
                {
                    throw new Exception("Ratings feedback not found!");
                }

                await _ratingsFeedbackRepository.DeleteAsync(feedbackId);
                await _ratingsFeedbackRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ratings feedback with ID {FeedbackId}: {ErrorMessage}", feedbackId, ex.Message);
                throw;
            }
        }
    }
}