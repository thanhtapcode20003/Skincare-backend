using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Data.DTO.Order
{
    public class CreateOrderForStaffDTO
    {
        public string UserId { get; set; }  // Customer placing the order
        public List<CreateOrderDetailDTO> OrderDetails { get; set; }
    }

    public class CreateOrderDetailDTO
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
