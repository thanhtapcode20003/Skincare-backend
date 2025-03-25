// IOrderService.cs
using SkinCare_Data.Data;
using SkinCare_Data.DTO.Order;
using SkinCare_Data.DTO.Orderdetai;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkinCare_Service.IService
{
    public interface IOrderService
    {
        Task<Order> AddToCartAsync(string userId, AddToCartDto addToCartDto);
        Task<List<OrderSummaryDto>> GetMyOrdersAsync(string userId);
        Task<List<OrderDetailResponseDto>> GetMyOrderDetailsAsync(string userId); // Thêm để xem OrderDetail
        Task<OrderDetailResponseDto> UpdateOrderDetailQuantityAsync(string userId, string orderDetailId, int quantityChange); // Thêm để thêm/bớt số lượng
        Task<List<OrderDetailResponseDto>> DeleteOrderDetailAsync(string userId, string orderDetailId); // Thêm để xóa OrderDetail
        Task<List<OrderStaffDTO>> GetAllOrdersForStaffAsync();
        Task<OrderStaffDTO> GetOrderDetailsForStaffAsync(string orderId);
        Task<OrderStaffDTO> CreateOrderForStaffAsync(CreateOrderForStaffDTO createOrderDto);
        Task<OrderStaffDTO> UpdateOrderStatusAsync(UpdateOrderStatusDTO updateOrderDto);
        Task<OrderStaffDTO> DeleteOrderAsync(string orderId);

    }
}