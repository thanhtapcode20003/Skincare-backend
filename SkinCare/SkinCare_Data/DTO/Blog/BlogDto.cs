using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Data.DTO.Blog
{
    public class BlogDto
    {
        public string BlogId { get; set; }
        public string UserId { get; set; }
        public string CategoryId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
