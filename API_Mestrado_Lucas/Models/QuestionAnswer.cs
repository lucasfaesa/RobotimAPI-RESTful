using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas.Models
{
    public class QuestionAnswer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string AnswerString { get; set; }
        public bool IsCorrectAnswer { get; set; }

        public virtual Question Question { get; set; }
    }
}
