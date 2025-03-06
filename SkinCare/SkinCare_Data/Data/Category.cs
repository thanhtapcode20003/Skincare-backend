using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Data.Data
{
    public class Category
    {
        [Key]
        public string CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public string? ParentCategoryId { get; set; }
        [ForeignKey(nameof(ParentCategoryId))]
        public Category ParentCategory { get; set; }

        public virtual ICollection<Category> SubCategories { get; set; }
    }
}