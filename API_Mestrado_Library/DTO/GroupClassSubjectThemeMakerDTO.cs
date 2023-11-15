using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas
{
    public class GroupClassSubjectThemeMakerDTO : DataTransferObject
    {
        private int groupClassId;
        private List<int> subjectThemesIds;

        public int GroupClassId { get => groupClassId; set => groupClassId = value; }
        public List<int> SubjectThemesIds { get => subjectThemesIds; set => subjectThemesIds = value; }
    }
}
