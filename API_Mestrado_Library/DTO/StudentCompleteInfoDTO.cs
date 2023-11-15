using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace API_Mestrado_Lucas
{
    public class StudentCompleteInfoDTO : DataTransferObject
    {
        private int id;
        private string username;
        private string password;
        private string name;
        private int? groupClassId;
        private DateTime creationDate;
        private DateTime lastLoginDate;
        private ICollection<SessionDTO> sessions;
        private int teacherId;
        private TeacherDTO teacher;
        private GroupClassDTO groupClass;

        public int Id { get => id; set => id = value; }
        public int TeacherId { get => teacherId; set => teacherId = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Name { get => name; set => name = value; }
        public int? GroupClassId { get => groupClassId; set => groupClassId = value; }
        public DateTime CreationDate { get => creationDate; set => creationDate = value; }
        public DateTime LastLoginDate { get => lastLoginDate; set => lastLoginDate = value; }


        public virtual ICollection<SessionDTO> Sessions { get => sessions; set => sessions = value; }


        public virtual TeacherDTO Teacher { get => teacher; set => teacher = value; }


        public virtual GroupClassDTO GroupClass { get => groupClass; set => groupClass = value; }
    }
}
