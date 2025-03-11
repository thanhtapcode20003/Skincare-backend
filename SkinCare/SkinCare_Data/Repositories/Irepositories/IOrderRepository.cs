// IOrderRepository.cs
using SkinCare_Data.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkinCare_Data.IRepositories
{
    public interface IOrderRepository
    {
        Task<Order> GetCartByUserIdAsync(string userId);
        Task<Order> GetByIdAsync(string orderId);
        Task<List<Order>> GetOrdersByUserIdAsync(string userId); // Thêm phương thức mới
        Task<int> GetOrderCountAsync();
        Task<int> GetOrderDetailCountAsync();
        Task AddOrderAsync(Order order);
        Task AddOrderDetailAsync(OrderDetail orderDetail);
        Task UpdateOrderAsync(Order order);
        Task SaveChangesAsync();
        SkinCare_DBContext GetContext();
    }
}