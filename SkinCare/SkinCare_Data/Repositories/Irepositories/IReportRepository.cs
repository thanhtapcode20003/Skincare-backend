using System.Collections.Generic;
using System.Threading.Tasks;
using SkinCare_Data.DTO.Report;

namespace SkinCare_Data.Repositories
{
    public interface IReportRepository
    {
        Task<List<RevenueReportDTO>> GetRevenueReportAsync(string type);
        Task<List<OrderStatusReportDTO>> GetOrdersByStatusReportAsync();
    }
}
