using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas.Models
{
    public class GroupClassSubjectTheme
    {
        public int GroupClassId { get; set; }
        public int SubjectThemeId { get; set; }

        public SubjectTheme SubjectTheme { get; set; }
        public GroupClass GroupClass { get; set; }
    }
}
