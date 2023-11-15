using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas
{
    public class StudentAndScoreDTO : DataTransferObject
    {
        private int studentId;
        private int score;
        private StudentDTO student;

        public int StudentId { get => studentId; set => studentId = value; }
        public int Score { get => score; set => score = value; }
        public virtual StudentDTO Student { get => student; set => student = value; }
    }
}
