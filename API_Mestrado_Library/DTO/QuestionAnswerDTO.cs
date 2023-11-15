using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas
{
    public class QuestionAnswerDTO : DataTransferObject
    {
        private int id;
        private int questionId;
        private string answerString;
        private bool isCorrectAnswer;
        private QuestionDTO question;

        public int Id { get => id; set => id = value; }
        public int QuestionId { get => questionId; set => questionId = value; }
        public string AnswerString { get => answerString; set => answerString = value; }
        public bool IsCorrectAnswer { get => isCorrectAnswer; set => isCorrectAnswer = value; }

        public QuestionDTO Question { get => question; set => question = value; }
    }
}
