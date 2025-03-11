using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Data.DTO.Order
{
    public class OrderSummaryDto
    {
        public string OrderId { get; set; }
        public string OrderStatus { get; set; }
        public int TotalAmount { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
