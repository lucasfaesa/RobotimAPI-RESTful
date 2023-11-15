using API_Mestrado_Lucas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas
{
    public class StudentAndCompleteLevelsSessionsDTO : DataTransferObject
    {
        private StudentDTO student;
        private List<CompleteLevelSessionDTO> completeLevelSessions;

        public StudentDTO StudentDto { get => student; set => student = value; }
        public List<CompleteLevelSessionDTO> CompleteLevelSessions { get => completeLevelSessions; set => completeLevelSessions = value; }
    }
}
