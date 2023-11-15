using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas
{
    public class StudentAndLevelIdDTO : DataTransferObject
    {
        private int studentId;
        private int levelId;

        public int StudentId { get => studentId; set => studentId = value; }
        public int LevelId { get => levelId; set => levelId = value; }
    }
}
