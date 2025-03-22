using System;
using System.Collections.Generic;

namespace SkinCare_Data.DTO.Order
{
    public class OrderStaffDTO
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public string OrderStatus { get; set; }
        public int TotalAmount { get; set; }
        public DateTime CreateAt { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; }
    }

    public class OrderDetailDTO
    {
        public string OrderDetailId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
    }
}
