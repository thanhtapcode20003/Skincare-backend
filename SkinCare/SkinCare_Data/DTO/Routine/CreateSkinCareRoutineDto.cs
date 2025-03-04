﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SkinCare_Data.DTO.Routine
{
    public class CreateSkinCareRoutineDto
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
