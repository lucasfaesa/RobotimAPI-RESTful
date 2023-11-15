using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace API_Mestrado_Lucas
{
    public class StudentDTO : DataTransferObject
    {
        private int id;
        private string name;
        private DateTime creationDate;
        private DateTime lastLoginDate;
        private int teacherId;
        private int? groupClassId;
        private GroupClassDTO groupClass;

        public int Id { get => id; set => id = value; }
        public int TeacherId { get => teacherId; set => teacherId = value; }
        public string Name { get => name; set => name = value; }
        public int? GroupClassId { get => groupClassId; set => groupClassId = value; }
        public DateTime CreationDate { get => creationDate; set => creationDate = value; }
        public DateTime LastLoginDate { get => lastLoginDate; set => lastLoginDate = value; }

        public virtual GroupClassDTO GroupClass { get => groupClass; set => groupClass = value; }

    }
}
