using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas.Models
{
    public class StudentWrongAnswers
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int QuizId { get; set; }
        public string QuestionTitle { get; set; }
        public string QuestionCorrectAnswer { get; set; }
        public string StudentWrongAnswer { get; set; }

        [JsonIgnore]
        public Student Student { get; set; }

        [JsonIgnore]
        public Quiz Quiz { get; set; }
    }
}
