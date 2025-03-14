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
        Task<List<Order>> GetOrdersByUserIdAsync(string userId);
        Task<List<OrderDetail>> GetOrderDetailsByUserIdAsync(string userId); // Thêm để lấy danh sách OrderDetail
        Task<OrderDetail> GetOrderDetailByIdAsync(string orderDetailId); // Thêm để lấy OrderDetail cụ thể
        Task<int> GetOrderCountAsync();
        Task<int> GetOrderDetailCountAsync();
        Task AddOrderAsync(Order order);
        Task AddOrderDetailAsync(OrderDetail orderDetail);
        Task UpdateOrderAsync(Order order);
        Task UpdateOrderDetailAsync(OrderDetail orderDetail); // Thêm để cập nhật OrderDetail
        Task DeleteOrderDetailAsync(string orderDetailId); // Thêm để xóa OrderDetail
        Task SaveChangesAsync();
        SkinCare_DBContext GetContext();
    }
}