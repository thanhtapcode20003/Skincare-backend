using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Data.DTO.RatingFeedBack
{
    public class RatingsFeedbackResponseDto
    {
        public string FeedbackId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
