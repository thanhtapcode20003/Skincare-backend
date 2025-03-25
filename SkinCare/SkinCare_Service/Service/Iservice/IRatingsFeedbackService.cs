using SkinCare_Data.DTO.RatingFeedBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Service.Service.Iservice
{
    public interface IRatingsFeedbackService
    {
        Task<RatingsFeedbackResponseDto> CreateRatingsFeedbackAsync(string userId, CreateRatingsFeedbackDto dto);
        Task<RatingsFeedbackResponseDto> GetRatingsFeedbackByIdAsync(string feedbackId);
        Task<List<RatingsFeedbackResponseDto>> GetRatingsFeedbackByProductIdAsync(string productId);
        Task<List<RatingsFeedbackResponseDto>> GetRatingsFeedbackByUserIdAsync(string userId);
        Task<RatingsFeedbackResponseDto> UpdateRatingsFeedbackAsync(string feedbackId, UpdateRatingsFeedbackDto dto);
        Task DeleteRatingsFeedbackAsync(string feedbackId);
    }
}
