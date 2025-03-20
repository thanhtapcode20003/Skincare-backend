using Microsoft.EntityFrameworkCore;
using SkinCare_Data;
using SkinCare_Data.DTO.Report;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkinCare_Data.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly SkinCare_DBContext _context;

        public ReportRepository(SkinCare_DBContext context)
        {
            _context = context;
        }

        public async Task<List<RevenueReportDTO>> GetRevenueReportAsync(string type)
        {
            var query = _context.Orders.AsQueryable();

            return type.ToLower() switch
            {
                "day" => await query
                    .GroupBy(o => o.CreateAt.Date)
                    .Select(g => new RevenueReportDTO
                    {
                        TimePeriod = g.Key.ToString("yyyy-MM-dd"),
                        TotalRevenue = g.Sum(o => o.TotalAmount)
                    })
                    .ToListAsync(),

                "month" => await query
                    .GroupBy(o => new { o.CreateAt.Year, o.CreateAt.Month })
                    .Select(g => new RevenueReportDTO
                    {
                        TimePeriod = $"{g.Key.Year}-{g.Key.Month}",
                        TotalRevenue = g.Sum(o => o.TotalAmount)
                    })
                    .ToListAsync(),

                "year" => await query
                    .GroupBy(o => o.CreateAt.Year)
                    .Select(g => new RevenueReportDTO
                    {
                        TimePeriod = g.Key.ToString(),
                        TotalRevenue = g.Sum(o => o.TotalAmount)
                    })
                    .ToListAsync(),

                _ => null
            };
        }

        public async Task<List<OrderStatusReportDTO>> GetOrdersByStatusReportAsync()
        {
            return await _context.Orders
                .GroupBy(o => o.OrderStatus)
                .Select(g => new OrderStatusReportDTO
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }
    }
}
