using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas.Models
{
    public class Level
    {
        public int Id { get; set; }
        public int SubjectThemeId { get; set; } //se for -1 eh porque nao tem tematica especifica, podem ser varias
        public int Difficulty { get; set; } //dificuldade de 1 a 10, -1 para casos em que o professor é livre para mudar perguntas da fase, ou seja, meio que nao tem dificuldade, ela é variavel
        public int MidScoreThreshold { get; set; }
        public int HighScoreThreshold { get; set; }

        [JsonIgnore]
        public virtual ICollection<Session> Sessions { get; set; }

        [JsonIgnore]
        public virtual SubjectTheme SubjectTheme { get; set; }

      
    }
}
