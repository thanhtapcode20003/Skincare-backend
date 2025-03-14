using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Data.DTO.Orderdetai
{
    public class OrderDetailResponseDto
    {
        public string OrderDetailId { get; set; }
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
    }

}
