using Microsoft.EntityFrameworkCore;
using SkinCare_Data.Data;
using SkinCare_Data.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkinCare_Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly SkinCare_DBContext _context;

        public CategoryRepository(SkinCare_DBContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories)
                .ToListAsync();
        }

        public async Task<Category> GetByIdAsync(string categoryId)
        {
            return await _context.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Categories.CountAsync();
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
        }

        public Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}