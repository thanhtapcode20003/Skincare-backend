using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Data.DTO.Category
{
    public class CreateCategoryDto
    {
        public string CategoryName { get; set; }
        public string? ParentCategoryId { get; set; }
    }
}
