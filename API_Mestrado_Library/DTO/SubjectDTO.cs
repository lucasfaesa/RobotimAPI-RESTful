using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace API_Mestrado_Lucas
{
    public class SubjectDTO : DataTransferObject
    {
        private int id;
        private string name;
        private ICollection<SubjectThemeDTO> subjectThemes;
        private ICollection<QuestionDTO> questions;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; } //matematica, portugues etc

        public virtual ICollection<SubjectThemeDTO> SubjectThemes { get => subjectThemes; set => subjectThemes = value; }

        public virtual ICollection<QuestionDTO> Questions { get => questions; set => questions = value; }
    }
}
