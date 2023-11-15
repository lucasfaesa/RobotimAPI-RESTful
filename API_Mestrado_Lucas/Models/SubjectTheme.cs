using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas.Models
{
    public class SubjectTheme
    {
        public int Id { get; set; } //se algum tiver subject theme como -1, eh porque nao eh nenhum tema em especifico (tipo uma prova, que envolve varios temas)
        public string Code { get; set; }
        public string Name { get; set; } //tipo matematica tem operacoes (soma, divisao) mas tambem tem ponto reta e plano, que eh outra tematica dentro da mesma materia
        public string Description { get; set; }
        public int SubjectId { get; set; } //se for -1 aqui eh pq nao representa materia nenhuma

        [JsonIgnore]
        public virtual ICollection<Level> Levels { get; set; }

        [JsonIgnore]
        public virtual Subject Subject { get; set; }

        [JsonIgnore]
        public ICollection<GroupClassSubjectTheme> GroupClassSubjectThemes { get; set; }

    }
}
