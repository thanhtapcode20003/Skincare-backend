using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Data.DTO.Dashboard
{
    public class DashboardDTO
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalCustomers { get; set; }
        public List<TopSellingProductDTO> TopSellingProducts { get; set; } = new();
    }

    public class TopSellingProductDTO
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
    }
}
