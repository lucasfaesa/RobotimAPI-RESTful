using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas
{
    public class TeacherCompleteInfoDTO : DataTransferObject
    {
        private int id;
        private string username;
        private string password;
        private string name;
        private DateTime creationDate;
        private DateTime lastLoginDate;
        private ICollection<StudentDTO> students;
        private ICollection<GroupClassDTO> groupClasses;

        public int Id { get => id; set => id = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Name { get => name; set => name = value; }
        public DateTime CreationDate { get => creationDate; set => creationDate = value; }
        public DateTime LastLoginDate { get => lastLoginDate; set => lastLoginDate = value; }


        public virtual ICollection<StudentDTO> Students { get => students; set => students = value; }

        public virtual ICollection<GroupClassDTO> GroupClasses { get => groupClasses; set => groupClasses = value; }
    }
}
