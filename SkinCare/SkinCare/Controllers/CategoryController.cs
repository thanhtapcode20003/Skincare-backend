using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkinCare_Service.IService;
using SkinCare_Data.Data;
using SkinCare_Data.DTO.Category;
using System.Threading.Tasks;

namespace SkinCare.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            try
            {
                _logger.LogInformation("Fetching all categories");
                var categories = await _categoryService.GetAllCategoriesAsync();
                if (categories == null || categories.Count == 0)
                {
                    return NotFound(new { message = "No categories found" });
                }
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching categories: {ErrorMessage}", ex.Message);
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(string id)
        {
            try
            {
                _logger.LogInformation("Fetching category with ID: {CategoryId}", id);
                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("Category not found with ID: {CategoryId}", id);
                    return NotFound(new { message = "Category not found" });
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching category with ID {CategoryId}: {ErrorMessage}", id, ex.Message);
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost("create")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<Category>> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            try
            {
                _logger.LogInformation("Creating new category with name: {CategoryName}", createCategoryDto.CategoryName);
                if (string.IsNullOrEmpty(createCategoryDto.CategoryName))
                {
                    return BadRequest(new { message = "CategoryName is required" });
                }
                var createdCategory = await _categoryService.CreateCategoryAsync(createCategoryDto);
                return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.CategoryId }, createdCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category with name {CategoryName}: {ErrorMessage}", createCategoryDto.CategoryName, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("edit/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateCategory(string id, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            try
            {
                _logger.LogInformation("Updating category with ID: {CategoryId}", id);
                if (string.IsNullOrEmpty(updateCategoryDto.CategoryName))
                {
                    return BadRequest(new { message = "CategoryName is required" });
                }
                var category = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);
                if (category == null)
                {
                    return NotFound(new { message = "Category not found" });
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category with ID {CategoryId}: {ErrorMessage}", id, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            try
            {
                _logger.LogInformation("Deleting category with ID: {CategoryId}", id);
                var result = await _categoryService.DeleteCategoryAsync(id);
                if (!result)
                {
                    return NotFound(new { message = "Category not found" });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category with ID {CategoryId}: {ErrorMessage}", id, ex.Message);
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}