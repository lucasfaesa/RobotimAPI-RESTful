using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace API_Mestrado_Lucas
{
    public class LevelDTO : DataTransferObject
    {
        private int id;
        private int subjectThemeId;
        private int difficulty;
        private ICollection<SessionDTO> sessions;
        private SubjectThemeDTO subjectTheme;
        private ICollection<GroupClassDTO> groupClasses;
        private ICollection<GroupClassSubjectThemeDTO> groupClassLevels;
        private int midScoreThreshold;
        private int highScoreThreshold;

        public int Id { get => id; set => id = value; }
        public int SubjectThemeId { get => subjectThemeId; set => subjectThemeId = value; }
        public int Difficulty { get => difficulty; set => difficulty = value; } //dificuldade de 1 a 10, 0 para casos em que o professor é livre para mudar perguntas da fase, ou seja, meio que nao tem dificuldade, ela é variavel
        public int MidScoreThreshold { get => midScoreThreshold; set => midScoreThreshold = value; }
        public int HighScoreThreshold { get => highScoreThreshold; set => highScoreThreshold = value; }


        public virtual ICollection<SessionDTO> Sessions { get => sessions; set => sessions = value; }

        public virtual SubjectThemeDTO SubjectTheme { get => subjectTheme; set => subjectTheme = value; }

        public virtual ICollection<GroupClassDTO> GroupClasses { get => groupClasses; set => groupClasses = value; }

        public ICollection<GroupClassSubjectThemeDTO> GroupClassLevels { get => groupClassLevels; set => groupClassLevels = value; }
    }
}

