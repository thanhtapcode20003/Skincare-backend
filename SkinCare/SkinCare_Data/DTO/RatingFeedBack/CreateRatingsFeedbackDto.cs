using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Data.DTO.RatingFeedBack
{
    public class CreateRatingsFeedbackDto
    {
        public string ProductId { get; set; }
        public int Rating { get; set; } // Từ 1 đến 5
        public string Comment { get; set; }
    }
}
