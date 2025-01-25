using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Data.Data
{
    public class RatingsFeedback
    {
        [Key]
        public string FeedbackId { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public string ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
