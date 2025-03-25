using Microsoft.EntityFrameworkCore;
using SkinCare_Data.Data;
using SkinCare_Data.IRepositories;
using SkinCare_Data.Repositories.Irepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkinCare_Data.Repositories
{
    public class RatingsFeedbackRepository : IRatingsFeedbackRepository
    {
        private readonly SkinCare_DBContext _context;

        public RatingsFeedbackRepository(SkinCare_DBContext context)
        {
            _context = context;
        }

        public async Task<RatingsFeedback> GetByIdAsync(string feedbackId)
        {
            return await _context.RatingsFeedbacks
                .Include(rf => rf.User)
                .Include(rf => rf.Product)
                .FirstOrDefaultAsync(rf => rf.FeedbackId == feedbackId);
        }

        public async Task<List<RatingsFeedback>> GetByProductIdAsync(string productId)
        {
            return await _context.RatingsFeedbacks
                .Include(rf => rf.User)
                .Include(rf => rf.Product)
                .Where(rf => rf.ProductId == productId)
                .ToListAsync();
        }

        public async Task<List<RatingsFeedback>> GetByUserIdAsync(string userId)
        {
            return await _context.RatingsFeedbacks
                .Include(rf => rf.User)
                .Include(rf => rf.Product)
                .Where(rf => rf.UserId == userId)
                .ToListAsync();
        }

        public async Task AddAsync(RatingsFeedback feedback)
        {
            await _context.RatingsFeedbacks.AddAsync(feedback);
        }

        public async Task UpdateAsync(RatingsFeedback feedback)
        {
            _context.RatingsFeedbacks.Update(feedback);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(string feedbackId)
        {
            var feedback = await _context.RatingsFeedbacks.FindAsync(feedbackId);
            if (feedback != null)
            {
                _context.RatingsFeedbacks.Remove(feedback);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}