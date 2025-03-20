using SkinCare_Data.DTO.Report;
using SkinCare_Data.Repositories;
using SkinCare_Service.Service.Iservice;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkinCare_Service.Service
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<List<RevenueReportDTO>> GetRevenueReportAsync(string type)
        {
            return await _reportRepository.GetRevenueReportAsync(type);
        }

        public async Task<List<OrderStatusReportDTO>> GetOrdersByStatusReportAsync()
        {
            return await _reportRepository.GetOrdersByStatusReportAsync();
        }
    }
}
