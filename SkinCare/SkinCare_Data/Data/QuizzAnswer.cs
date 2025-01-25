using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Data.Data
{
    public class QuizzAnswer
    {
        [Key]
        public string QuizAnswerId { get; set; }

        public string QuizId { get; set; }
        [ForeignKey(nameof(QuizId))]
        public Quizz Quizz { get; set; }

        public string Answer { get; set; }
        public int Score { get; set; }
    }
}
