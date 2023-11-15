using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastLoginDate { get; set; }

        [JsonIgnore]
        public virtual ICollection<Student> Students { get; set; }

        [JsonIgnore]
        public virtual ICollection<GroupClass> GroupClasses { get; set; }

        [JsonIgnore]
        public virtual ICollection<Question> Questions { get; set; }
    }
}
