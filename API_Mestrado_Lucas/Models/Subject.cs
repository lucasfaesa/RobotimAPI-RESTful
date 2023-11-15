using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas.Models
{
    public class Subject
    {
        public int Id { get; set; } //subject com id -1 eh porque nao eh uma materia especifica (usado pelo subjectheme)
        public string Name { get; set; } //matematica, portugues etc

        [JsonIgnore]
        public virtual ICollection<SubjectTheme> SubjectThemes { get; set; }

    }
}
