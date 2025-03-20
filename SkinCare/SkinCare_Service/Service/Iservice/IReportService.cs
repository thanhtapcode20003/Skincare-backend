using SkinCare_Data.DTO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Service.Service.Iservice
{
    public interface IReportService
    {
        Task<List<RevenueReportDTO>> GetRevenueReportAsync(string type);
        Task<List<OrderStatusReportDTO>> GetOrdersByStatusReportAsync();
    }
}
