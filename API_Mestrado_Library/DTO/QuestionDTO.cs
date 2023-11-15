using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas
{
    public class QuestionDTO : DataTransferObject
    {
        private int id;
        private string questionTitle;
        private float questionTimeLimit;
        private ICollection<QuestionAnswerDTO> questionAnswers;
        private float questionScoreValue;
        private QuizDTO quiz;
        private int quizId;

        public int Id { get => id; set => id = value; }
        public string QuestionTitle { get => questionTitle; set => questionTitle = value; }
        public float QuestionTimeLimit { get => questionTimeLimit; set => questionTimeLimit = value; }
        public float QuestionScoreValue { get => questionScoreValue; set => questionScoreValue = value; }
        public int QuizId { get => quizId; set => quizId = value; }
        public ICollection<QuestionAnswerDTO> QuestionAnswers { get => questionAnswers; set => questionAnswers = value; }

        public QuizDTO Quiz { get => quiz; set => quiz = value; }
    }
}
