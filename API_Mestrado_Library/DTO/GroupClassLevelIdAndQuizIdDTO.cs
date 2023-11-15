using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas
{
    public class GroupClassLevelIdAndQuizIdDTO : DataTransferObject
    {
        private int groupClassId;
        private int? levelId;
        private int? quizId;

        public int GroupClassId { get => groupClassId; set => groupClassId = value; }
        public int? LevelId { get => levelId; set => levelId = value; }
        public int? QuizId { get => quizId; set => quizId = value; }
    }
}
