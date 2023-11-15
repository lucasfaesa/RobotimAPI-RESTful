using API_Mestrado_Lucas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas.Services.Interfaces
{
    public interface ISessionService
    {
        Task<IEnumerable<StudentAndCompleteLevelsSessionsDTO>> CompleteStudentsLevelSessionByGroupClassId(int id);
        Task<IEnumerable<CompleteStudentSessionDTO>> CompleteStudentsSessionsByTeacherAndGroupClass(TeacherIdAndGroupClassIdDTO dto);
        Task<IEnumerable<StudentAndScoreDTO>> GetHighScoresOfGroupClass(int groupClassId);
        Task<IEnumerable<Session>> GetHighScoreByLevelIdOrQuizIdAndGroupClass(GroupClassLevelIdAndQuizIdDTO obj);
        Task<IEnumerable<Session>> GetStudentLevelsTopScores(int id);
        Task<IEnumerable<Session>> GetStudentQuizesTopScores(int id);
        Task<Session> GetStudentTopScoreByLevel(StudentAndLevelIdDTO obj);
        Task<Session> CreateSession(SessionDTO dto);
    }
}
