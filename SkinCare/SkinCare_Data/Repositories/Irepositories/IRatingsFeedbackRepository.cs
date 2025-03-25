using SkinCare_Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Data.Repositories.Irepositories
{
    public interface IRatingsFeedbackRepository
    {
        Task<RatingsFeedback> GetByIdAsync(string feedbackId);
        Task<List<RatingsFeedback>> GetByProductIdAsync(string productId);
        Task<List<RatingsFeedback>> GetByUserIdAsync(string userId);
        Task AddAsync(RatingsFeedback feedback);
        Task UpdateAsync(RatingsFeedback feedback);
        Task DeleteAsync(string feedbackId);
        Task SaveChangesAsync();
    }
}
