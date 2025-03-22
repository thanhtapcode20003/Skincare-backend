using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Data.Data
{
    public class Order
    {
        [Key]
        public string OrderId { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public string OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreateAt { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }

}
