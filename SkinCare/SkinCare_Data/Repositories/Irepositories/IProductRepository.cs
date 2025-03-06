using SkinCare_Data.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkinCare_Data.IRepositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(string productId);
        Task<int> GetCountAsync();
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task SaveChangesAsync();
    }
}