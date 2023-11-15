using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas
{
    public class StudentWrongAnswersDTO : DataTransferObject
    {
        private int id;
        private int studentId;
        private string questionTitle;
        private string questionCorrectAnswer;
        private string studentWrongAnswer;
        private StudentDTO student;
        private int quizId;
        private QuizDTO quiz;

        public int Id { get => id; set => id = value; }
        public int StudentId { get => studentId; set => studentId = value; }
        public int QuizId { get => quizId; set => quizId = value; }
        public string QuestionTitle { get => questionTitle; set => questionTitle = value; }
        public string QuestionCorrectAnswer { get => questionCorrectAnswer; set => questionCorrectAnswer = value; }
        public string StudentWrongAnswer { get => studentWrongAnswer; set => studentWrongAnswer = value; }


        public StudentDTO Student { get => student; set => student = value; }
        public QuizDTO Quiz { get => quiz; set => quiz = value; }
    }
}
