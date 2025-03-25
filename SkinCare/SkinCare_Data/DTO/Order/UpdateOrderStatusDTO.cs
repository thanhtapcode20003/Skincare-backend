using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Data.DTO.Order
{
    public class UpdateOrderStatusDTO
    {
        public string OrderId { get; set; }
        public string NewStatus { get; set; }
    }
}
