// SkinCare_Service/IService/IOrderService.cs
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
        Task<List<OrderDetailResponseDto>> GetMyOrderDetailsAsync(string userId);
        Task<OrderDetailResponseDto> UpdateOrderDetailQuantityAsync(string userId, string orderDetailId, int quantityChange);
        Task<List<OrderDetailResponseDto>> DeleteOrderDetailAsync(string userId, string orderDetailId);
        Task HandleCODPaymentAsync(string userId, string orderId);
        Task<string> CreateVNPayPaymentUrlAsync(string userId, string orderId, string ipAddress); // Thêm để tạo URL thanh toán
        Task<bool> HandleVNPayCallbackAsync(Dictionary<string, string> vnpayData); // Thêm để xử lý callback
    }
}