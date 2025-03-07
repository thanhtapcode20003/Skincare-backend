using SkinCare_Data.Data;
using SkinCare_Data.DTO.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkinCare_Service.IService
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(string categoryId);
        Task<Category> CreateCategoryAsync(CreateCategoryDto createCategoryDto); 
        Task<Category> UpdateCategoryAsync(string categoryId, UpdateCategoryDto updateCategoryDto); 
        Task<bool> DeleteCategoryAsync(string categoryId);
    }
}