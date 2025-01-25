using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Data.Data
{
    public class SkinCareRoutine
    {
        [Key]
        public string RoutineId { get; set; }

        public string Description { get; set; }
        public string Type { get; set; }
    }
}
