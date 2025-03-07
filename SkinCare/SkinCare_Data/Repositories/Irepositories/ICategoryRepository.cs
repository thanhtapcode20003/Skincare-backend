using SkinCare_Data.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkinCare_Data.IRepositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(string categoryId);
        Task<int> GetCountAsync();
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Category category);
        Task SaveChangesAsync();
    }
}