using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas
{
    public class TeacherDTO : DataTransferObject
    {
        private int id;
        private string name;
        private DateTime creationDate;
        private DateTime lastLoginDate;
        private ICollection<StudentCompleteInfoDTO> students;
        private ICollection<GroupClassDTO> groupClasses;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public DateTime CreationDate { get => creationDate; set => creationDate = value; }
        public DateTime LastLoginDate { get => lastLoginDate; set => lastLoginDate = value; }

        public virtual ICollection<StudentCompleteInfoDTO> Students { get => students; set => students = value; }

        public virtual ICollection<GroupClassDTO> GroupClasses { get => groupClasses; set => groupClasses = value; }
    }
}
