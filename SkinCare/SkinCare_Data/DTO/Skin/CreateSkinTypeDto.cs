using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace SkinCare_Data.DTO.Skin
{
    public class CreateSkinTypeDto
    {
        [Required]
        public string SkinTypeName { get; set; }

        public string Description { get; set; }

        public string RoutineId { get; set; } // Tùy chọn, có thể null
    }
}
