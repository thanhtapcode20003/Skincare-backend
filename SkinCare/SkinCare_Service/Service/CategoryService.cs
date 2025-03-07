using Microsoft.Extensions.Logging;
using SkinCare_Data.Data;
using SkinCare_Data.IRepositories;
using SkinCare_Service.IService;
using SkinCare_Data.DTO.Category;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkinCare_Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository repository, ILogger<CategoryService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all categories");
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching categories: {ErrorMessage}", ex.Message);
                throw;
            }
        }

        public async Task<Category> GetCategoryByIdAsync(string categoryId)
        {
            try
            {
                _logger.LogInformation("Fetching category with ID: {CategoryId}", categoryId);
                var category = await _repository.GetByIdAsync(categoryId);
                if (category == null)
                {
                    _logger.LogWarning("Category not found with ID: {CategoryId}", categoryId);
                    return null;
                }
                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching category with ID {CategoryId}: {ErrorMessage}", categoryId, ex.Message);
                throw;
            }
        }

        public async Task<Category> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            try
            {
                _logger.LogInformation("Creating new category with name: {CategoryName}", createCategoryDto.CategoryName);

                if (string.IsNullOrEmpty(createCategoryDto.CategoryName))
                {
                    throw new Exception("CategoryName is required!");
                }

                // Kiểm tra nếu ParentCategoryId tồn tại (nếu có)
                if (createCategoryDto.ParentCategoryId != null)
                {
                    var parentCategory = await _repository.GetByIdAsync(createCategoryDto.ParentCategoryId);
                    if (parentCategory == null)
                    {
                        throw new Exception("Parent category not found!");
                    }
                }

                // Tạo đối tượng Category từ DTO
                var category = new Category
                {
                    CategoryName = createCategoryDto.CategoryName,
                    ParentCategoryId = createCategoryDto.ParentCategoryId
                };

                int categoryCount = await _repository.GetCountAsync();
                category.CategoryId = $"CAT{(categoryCount + 1):D3}";

                await _repository.AddAsync(category);
                await _repository.SaveChangesAsync();

                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category with name {CategoryName}: {ErrorMessage}", createCategoryDto.CategoryName, ex.Message);
                throw;
            }
        }

        public async Task<Category> UpdateCategoryAsync(string categoryId, UpdateCategoryDto updateCategoryDto)
        {
            try
            {
                _logger.LogInformation("Updating category with ID: {CategoryId}", categoryId);

                var existingCategory = await _repository.GetByIdAsync(categoryId);
                if (existingCategory == null)
                {
                    throw new Exception("Category not found!");
                }

                // Kiểm tra nếu ParentCategoryId tồn tại (nếu có)
                if (updateCategoryDto.ParentCategoryId != null)
                {
                    var parentCategory = await _repository.GetByIdAsync(updateCategoryDto.ParentCategoryId);
                    if (parentCategory == null)
                    {
                        throw new Exception("Parent category not found!");
                    }
                }

                existingCategory.CategoryName = updateCategoryDto.CategoryName ?? existingCategory.CategoryName;
                existingCategory.ParentCategoryId = updateCategoryDto.ParentCategoryId;

                await _repository.UpdateAsync(existingCategory);
                await _repository.SaveChangesAsync();

                return existingCategory;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category with ID {CategoryId}: {ErrorMessage}", categoryId, ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteCategoryAsync(string categoryId)
        {
            try
            {
                _logger.LogInformation("Deleting category with ID: {CategoryId}", categoryId);

                var category = await _repository.GetByIdAsync(categoryId);
                if (category == null)
                {
                    _logger.LogWarning("Category not found with ID: {CategoryId}", categoryId);
                    return false;
                }

                await _repository.DeleteAsync(category);
                await _repository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category with ID {CategoryId}: {ErrorMessage}", categoryId, ex.Message);
                throw;
            }
        }
    }
}