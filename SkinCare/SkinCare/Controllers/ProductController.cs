﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkinCare_Data.Data;
using SkinCare_Data.DTO;
using SkinCare_Data.DTO.Product;
using SkinCare_Service.IService; // Thêm namespace để sử dụng IProductService
using System.Threading.Tasks;

[Route("api/products")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductService productService, ILogger<ProductController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    // GET: api/products (Không yêu cầu authorize, bất kỳ ai cũng có thể truy cập)
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts()
    {
        try
        {
            _logger.LogInformation("Fetching all products");

            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching products: {ErrorMessage}", ex.Message);
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }

    // GET: api/products/{id} (Không yêu cầu authorize, bất kỳ ai cũng có thể truy cập)
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(string id)
    {
        try
        {
            _logger.LogInformation("Fetching product with ID: {ProductId}", id);

            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product not found with ID: {ProductId}", id);
                return NotFound(new { message = "Product not found" });
            }

            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching product with ID {ProductId}: {ErrorMessage}", id, ex.Message);
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }

    // POST: api/products/create (Chỉ Staff mới có thể truy cập, sử dụng CreateProductDto)
    [HttpPost("create")]
    [Authorize(Roles = "Staff")]
    public async Task<ActionResult<Product>> CreateProduct(CreateProductDto createProductDto)
    {
        try
        {
            _logger.LogInformation("Creating new product with name: {ProductName}", createProductDto.ProductName);

            if (string.IsNullOrEmpty(createProductDto.ProductName))
            {
                return BadRequest(new { message = "ProductName is required" });
            }
            if (string.IsNullOrEmpty(createProductDto.Description))
            {
                return BadRequest(new { message = "Description is required" });
            }

            var createdProduct = await _productService.CreateProductAsync(createProductDto);
            return Ok(createdProduct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product with name {ProductName}: {ErrorMessage}", createProductDto.ProductName, ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/products/edit/{id} (Chỉ Staff mới có thể truy cập, sử dụng UpdateProductDto)
    [HttpPut("edit/{id}")]
    [Authorize(Roles = "Staff")]
    public async Task<IActionResult> UpdateProduct(string id, UpdateProductDto updateProductDto)
    {
        try
        {
            _logger.LogInformation("Updating product with ID: {ProductId}", id);

            // Lưu ý: Kiểm tra ProductId thay vì ProductName (sửa logic kiểm tra)
           // if (id != updateProductDto.ProductId) // Sử dụng ProductId từ DTO
           // {
             //   return BadRequest(new { message = "Product ID mismatch" });
          //  }

            var updatedProduct = await _productService.UpdateProductAsync(id, updateProductDto);
            return Ok(updatedProduct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product with ID {ProductId}: {ErrorMessage}", id, ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/products/delete/{id} (Chỉ Staff mới có thể truy cập)
    [HttpDelete("delete/{id}")]
    [Authorize(Roles = "Staff")]
    public async Task<IActionResult> DeleteProduct(string id)
    {
        try
        {
            _logger.LogInformation("Deleting product with ID: {ProductId}", id);

            var result = await _productService.DeleteProductAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Product not found" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product with ID {ProductId}: {ErrorMessage}", id, ex.Message);
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });
        }
    }
}