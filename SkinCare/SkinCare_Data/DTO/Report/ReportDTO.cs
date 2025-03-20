using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Data.DTO.Report
{
    public class RevenueReportDTO
    {
        public string TimePeriod { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class OrderStatusReportDTO
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }
}
