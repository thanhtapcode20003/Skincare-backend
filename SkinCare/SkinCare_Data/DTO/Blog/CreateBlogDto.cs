using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Data.DTO.Blog
{
    public class CreateBlogDto
    {
        public string CategoryId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}

