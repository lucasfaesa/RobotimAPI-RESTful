using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? TeacherId { get; set; }
        public bool Active { get; set; } //se estiver ativo irá aparecer para o aluno, caso contrário, não

        public virtual Teacher Teacher { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
        
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
