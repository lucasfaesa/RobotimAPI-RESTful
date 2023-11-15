using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas
{
    public class GroupClassSubjectThemeDTO
    {
        private int groupClassId;
        private int subjectThemeId;
        private SubjectThemeDTO subjectTheme;
        private GroupClassDTO groupClass;

        public int GroupClassId { get => groupClassId; set => groupClassId = value; }
        public int SubjectThemeId { get => subjectThemeId; set => subjectThemeId = value; }

        public SubjectThemeDTO SubjectTheme { get => subjectTheme; set => subjectTheme = value; }
        public GroupClassDTO GroupClass { get => groupClass; set => groupClass = value; }
    }
}
