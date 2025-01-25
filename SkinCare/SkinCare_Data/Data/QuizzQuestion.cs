using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Data.Data
{
    public class QuizzQuestion
    {
        [Key] // Explicitly mark QuestionId as the primary key
        public string QuestionId { get; set; }

        public string QuizzId { get; set; }

        [ForeignKey(nameof(QuizzId))]
        public Quizz Quizz { get; set; }

        public string Question { get; set; }
    }

}
