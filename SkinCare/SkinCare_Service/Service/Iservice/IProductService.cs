using SkinCare_Data.Data;
using SkinCare_Data.DTO;
using SkinCare_Data.DTO.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkinCare_Service.IService
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(string productId);
        Task<Product> CreateProductAsync(CreateProductDto createProductDto);
        Task<Product> UpdateProductAsync(string productId, UpdateProductDto updateProductDto);
        Task<bool> DeleteProductAsync(string productId);
    }
}