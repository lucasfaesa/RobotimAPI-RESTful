using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string QuestionTitle { get; set; }
        public float QuestionTimeLimit { get; set; }
        public int QuestionScoreValue { get; set; }
        public int QuizId { get; set; }

        [JsonIgnore]
        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }

        [JsonIgnore]
        public virtual Quiz Quiz { get; set; }
    }
}
