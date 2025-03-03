using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkinCare_Data;
using SkinCare_Data.Data;
using SkinCare_Data.DTO;
using SkinCare_Data.DTO.Product;
using System.Threading.Tasks;

public class ProductService
{
    private readonly SkinCare_DBContext _context;
    private readonly ILogger<ProductService> _logger;

    public ProductService(SkinCare_DBContext context, ILogger<ProductService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Get all products
    public async Task<List<Product>> GetAllProductsAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all products");

            var products = await _context.Products
                .Include(p => p.SkinType)
                .Include(p => p.Category)
                .Include(p => p.Routine)
                .ToListAsync();

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

            var product = await _context.Products
                .Include(p => p.SkinType)
                .Include(p => p.Category)
                .Include(p => p.Routine)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

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
                RatingFeedback = null, // Mặc định null vì không có trong DTO
                CreateAt = DateTime.UtcNow // Tự động gán thời gian tạo
            };

            // Tự động tạo ProductId (ví dụ: P001, P002, ...)
            int productCount = await _context.Products.CountAsync();
            product.ProductId = $"P{(productCount + 1):D3}";

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

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

            var existingProduct = await _context.Products.FindAsync(productId);
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
            existingProduct.RatingFeedback = null; // Không cập nhật RatingFeedback vì không có trong DTO

            // Không cập nhật CreateAt (giữ nguyên thời gian tạo)

            await _context.SaveChangesAsync();

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

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                _logger.LogWarning("Product not found with ID: {ProductId}", productId);
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product with ID {ProductId}: {ErrorMessage}", productId, ex.Message);
            throw;
        }
    }
}