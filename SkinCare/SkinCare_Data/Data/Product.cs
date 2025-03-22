using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Data.Data
{
    public class Product
    {
        [Key]
        public string ProductId { get; set; }

        public string SkinTypeId { get; set; }
        [ForeignKey(nameof(SkinTypeId))]
        public SkinType SkinType { get; set; }

        public string CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        public string RoutineId { get; set; }
        [ForeignKey(nameof(RoutineId))]
        public SkinCareRoutine Routine { get; set; }

        [Required]
        public string ProductName { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        public DateTime CreateAt { get; set; }

       
        public virtual ICollection<RatingsFeedback> RatingsFeedbacks { get; set; }
    }
}