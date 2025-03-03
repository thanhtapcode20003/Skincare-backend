using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SkinCare_Data.DTO.Product
{
    public class CreateProductDto
    {
        [Required]
        public string ProductName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string SkinTypeId { get; set; }

        public string CategoryId { get; set; }

        public string RoutineId { get; set; }

        public string Image { get; set; }
    }
}
