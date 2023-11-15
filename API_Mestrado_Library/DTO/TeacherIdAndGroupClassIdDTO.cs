using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas
{
    public class TeacherIdAndGroupClassIdDTO : DataTransferObject
    {
        private int teacherId;
        private int groupClassId;

        public int TeacherId { get => teacherId; set => teacherId = value; }
        public int GroupClassId { get => groupClassId; set => groupClassId = value; }
    }
}
