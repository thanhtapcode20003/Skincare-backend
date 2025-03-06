using Microsoft.EntityFrameworkCore;
using SkinCare_Data.Data;
using SkinCare_Data.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkinCare_Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SkinCare_DBContext _context;

        public ProductRepository(SkinCare_DBContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.SkinType)
                .Include(p => p.Category)
                .Include(p => p.Routine)
                .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(string productId)
        {
            return await _context.Products
                .Include(p => p.SkinType)
                .Include(p => p.Category)
                .Include(p => p.Routine)
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Products.CountAsync();
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}