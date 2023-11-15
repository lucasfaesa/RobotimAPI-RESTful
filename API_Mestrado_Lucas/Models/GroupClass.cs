using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas.Models
{
    public class GroupClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TeacherId { get; set; }


        [JsonIgnore]
        public virtual Teacher Teacher { get; set; }

        [JsonIgnore]
        public virtual ICollection<Student> Students { get; set; }

        [JsonIgnore]
        public ICollection<GroupClassSubjectTheme> GroupClassSubjectThemes { get; set; }
    }
}
