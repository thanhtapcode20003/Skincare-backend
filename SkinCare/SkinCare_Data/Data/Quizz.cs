using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Data.Data
{
    public class Quizz
    {
        [Key]
        public string QuizzId { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public string SkinTypeId { get; set; }
        [ForeignKey(nameof(SkinTypeId))]
        public SkinType SkinType { get; set; }

        public int TotalScore { get; set; }
        public DateTime TestDate { get; set; }

        public ICollection<QuizzAnswer> QuizzAnswers { get; set; }
        public ICollection<QuizzQuestion> QuizzQuestions { get; set; }
    }

}
