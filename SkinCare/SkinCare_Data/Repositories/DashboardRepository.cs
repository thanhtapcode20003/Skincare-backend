using Microsoft.EntityFrameworkCore;
using SkinCare_Data;
using SkinCare_Data.DTO.Dashboard;
using System.Linq;
using System.Threading.Tasks;

namespace SkinCare_Data.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly SkinCare_DBContext _context;

        public DashboardRepository(SkinCare_DBContext context)
        {
            _context = context;
        }

        public async Task<DashboardDTO> GetDashboardOverviewAsync()
        {
            var totalOrders = await _context.Orders.CountAsync();

            var totalRevenue = await _context.OrderDetails
                .SumAsync(od => (decimal?)od.Price * od.Quantity) ?? 0;

            var totalCustomers = await _context.Users.CountAsync();

            var topSellingProducts = await _context.OrderDetails
                .Include(od => od.Product)
                .GroupBy(od => new { od.ProductId, od.Product.ProductName })
                .Select(g => new TopSellingProductDTO
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    QuantitySold = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(p => p.QuantitySold)
                .Take(5)
                .ToListAsync();

            return new DashboardDTO
            {
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
                TotalCustomers = totalCustomers,
                TopSellingProducts = topSellingProducts
            };
        }
    }
}
