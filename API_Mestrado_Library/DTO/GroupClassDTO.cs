using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas
{
    public class GroupClassDTO
    {
        private int id;
        private string name;
        private int teacherId;
        private TeacherDTO teacher;
        private ICollection<StudentDTO> students;
        private ICollection<GroupClassSubjectThemeDTO> groupClassSubjectThemes;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int TeacherId { get => teacherId; set => teacherId = value; }


        public virtual TeacherDTO Teacher { get => teacher; set => teacher = value; }

        public virtual ICollection<StudentDTO> Students { get => students; set => students = value; }

        public ICollection<GroupClassSubjectThemeDTO> GroupClassSubjectThemes { get => groupClassSubjectThemes; set => groupClassSubjectThemes = value; }
    }
}
