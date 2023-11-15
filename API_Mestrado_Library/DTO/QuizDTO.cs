using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas
{
    public class QuizDTO : DataTransferObject
    {
        private int id;
        private string name;
        private int? teacherId;
        private TeacherDTO teacher;
        private ICollection<QuestionDTO> questions;
        private bool active;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int? TeacherId { get => teacherId; set => teacherId = value; }
        public bool Active { get => active; set => active = value; } //se estiver ativo irá aparecer para o aluno, caso contrário, não
        public virtual TeacherDTO Teacher { get => teacher; set => teacher = value; }

        public virtual ICollection<QuestionDTO> Questions { get => questions; set => questions = value; }
    }
}
