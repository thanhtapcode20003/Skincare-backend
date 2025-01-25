using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    }
}
