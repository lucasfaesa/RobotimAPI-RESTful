using API_Mestrado_Lucas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas
{
    public class StudentEditInfoDTO : DataTransferObject
    {
        private int id;
        private string username;
        private string password;
        private string name;
        private int teacherId;
        private int groupClassId;

        public int Id { get => id; set => id = value; }
        public int TeacherId { get => teacherId; set => teacherId = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Name { get => name; set => name = value; }
        public int GroupClassId { get => groupClassId; set => groupClassId = value; }
    }
}
