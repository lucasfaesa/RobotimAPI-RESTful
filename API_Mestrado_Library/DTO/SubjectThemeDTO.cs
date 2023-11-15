using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace API_Mestrado_Lucas
{
    public class SubjectThemeDTO : DataTransferObject
    {
        private int id;
        private string code;
        private string name;
        private string description;
        private int subjectId;
        private SubjectDTO subject;
        private ICollection<LevelDTO> levels;

        public int Id { get => id; set => id = value; } //se algum tiver subject theme como 0, eh porque nao eh nenhum tema em especifico (tipo uma prova, que envolve varios temas)
        public string Name { get => name; set => name = value; } //tipo matematica tem operacoes (soma, divisao) mas tambem tem ponto reta e plano, que eh outra tematica dentro da mesma materia
        public string Code { get => code; set => code = value; }
        public string Description { get => description; set => description = value; }
        public int SubjectId { get => subjectId; set => subjectId = value; }

        public virtual ICollection<LevelDTO> Levels { get => levels; set => levels = value; }

        public virtual SubjectDTO Subject { get => subject; set => subject = value; }

    }
}
