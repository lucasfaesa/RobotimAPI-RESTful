using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas.Models
{
    public class Student
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int? GroupClassId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastLoginDate { get; set; }

        [JsonIgnore]
        public virtual ICollection<Session> Sessions { get; set; }

        [JsonIgnore]
        public virtual Teacher Teacher { get; set; }

        [JsonIgnore]
        public virtual GroupClass GroupClass { get; set; }
    }
}
