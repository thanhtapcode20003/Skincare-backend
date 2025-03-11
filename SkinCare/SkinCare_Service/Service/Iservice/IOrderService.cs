// IOrderService.cs
using SkinCare_Data.Data;
using SkinCare_Data.DTO.Order;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkinCare_Service.IService
{
    public interface IOrderService
    {
        Task<Order> AddToCartAsync(string userId, AddToCartDto addToCartDto);
        Task<List<OrderSummaryDto>> GetMyOrdersAsync(string userId); 
    }
}