using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Data.Data
{
    public class SkinType
    {
        [Key]
        public string SkinTypeId { get; set; }

        public string RoutineId { get; set; }
        [ForeignKey(nameof(RoutineId))]
        public SkinCareRoutine SkinCareRoutine { get; set; }

        [Required]
        public string SkinTypeName { get; set; }

        public string Description { get; set; }
    }
}
