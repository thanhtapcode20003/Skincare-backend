using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkinCare_Data.Data;
using SkinCare_Data.DTO;
using SkinCare_Data.DTO.Product;
using SkinCare_Data.IRepositories;
using SkinCare_Service.IService;
using System;
using System.Threading.Tasks;

namespace SkinCare_Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository repository, ILogger<ProductService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // Get all products
        public async Task<List<Product>> GetAllProductsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all products");

                var products = await _repository.GetAllAsync();
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products: {ErrorMessage}", ex.Message);
                throw;
            }
        }

        // Get product by ID
        public async Task<Product> GetProductByIdAsync(string productId)
        {
            try
            {
                _logger.LogInformation("Fetching product with ID: {ProductId}", productId);

                var product = await _repository.GetByIdAsync(productId);
                if (product == null)
                {
                    _logger.LogWarning("Product not found with ID: {ProductId}", productId);
                    return null;
                }

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product with ID {ProductId}: {ErrorMessage}", productId, ex.Message);
                throw;
            }
        }

        // Create product (using CreateProductDto, chỉ yêu cầu ProductName, Description, Price, Quantity)
        public async Task<Product> CreateProductAsync(CreateProductDto createProductDto)
        {
            try
            {
                _logger.LogInformation("Creating new product with name: {ProductName}", createProductDto.ProductName);

                if (string.IsNullOrEmpty(createProductDto.ProductName))
                {
                    throw new Exception("ProductName is required!");
                }
                if (string.IsNullOrEmpty(createProductDto.Description))
                {
                    throw new Exception("Description is required!");
                }

                // Tạo đối tượng Product từ DTO
                var product = new Product
                {
                    ProductName = createProductDto.ProductName,
                    Description = createProductDto.Description,
                    Price = createProductDto.Price,
                    Quantity = createProductDto.Quantity,
                    SkinTypeId = createProductDto.SkinTypeId,
                    CategoryId = createProductDto.CategoryId,
                    RoutineId = createProductDto.RoutineId,
                    Image = createProductDto.Image,
                    CreateAt = DateTime.UtcNow // Tự động gán thời gian tạo
                };

                // Tự động tạo ProductId (ví dụ: P001, P002, ...)
                int productCount = await _repository.GetCountAsync();
                product.ProductId = $"P{(productCount + 1):D3}";

                await _repository.AddAsync(product);
                await _repository.SaveChangesAsync();

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product with name {ProductName}: {ErrorMessage}", createProductDto.ProductName, ex.Message);
                throw;
            }
        }

        // Update product (using UpdateProductDto)
        public async Task<Product> UpdateProductAsync(string productId, UpdateProductDto updateProductDto)
        {
            try
            {
                _logger.LogInformation("Updating product with ID: {ProductId}", productId);

                var existingProduct = await _repository.GetByIdAsync(productId);
                if (existingProduct == null)
                {
                    throw new Exception("Product not found!");
                }

                // Cập nhật tất cả các trường từ DTO, ngoại trừ CreateAt và ProductId
                existingProduct.ProductName = updateProductDto.ProductName ?? existingProduct.ProductName;
                existingProduct.Description = updateProductDto.Description ?? existingProduct.Description;
                existingProduct.Price = updateProductDto.Price;
                existingProduct.Quantity = updateProductDto.Quantity;
                existingProduct.SkinTypeId = updateProductDto.SkinTypeId;
                existingProduct.CategoryId = updateProductDto.CategoryId;
                existingProduct.RoutineId = updateProductDto.RoutineId;
                existingProduct.Image = updateProductDto.Image ?? existingProduct.Image;

                // Không cập nhật CreateAt (giữ nguyên thời gian tạo)

                await _repository.UpdateAsync(existingProduct);
                await _repository.SaveChangesAsync();

                return existingProduct;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product with ID {ProductId}: {ErrorMessage}", productId, ex.Message);
                throw;
            }
        }

        // Delete product
        public async Task<bool> DeleteProductAsync(string productId)
        {
            try
            {
                _logger.LogInformation("Deleting product with ID: {ProductId}", productId);

                var product = await _repository.GetByIdAsync(productId);
                if (product == null)
                {
                    _logger.LogWarning("Product not found with ID: {ProductId}", productId);
                    return false;
                }

                await _repository.DeleteAsync(product);
                await _repository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product with ID {ProductId}: {ErrorMessage}", productId, ex.Message);
                throw;
            }
        }
    }
}