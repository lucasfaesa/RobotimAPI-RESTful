using API_Mestrado_Lucas.Context;
using API_Mestrado_Lucas.Models;
using API_Mestrado_Lucas.Services.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas.Services
{
    public class SessionService : ISessionService
    {

        private readonly AppDbContext _context;

        public SessionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentAndCompleteLevelsSessionsDTO>> CompleteStudentsLevelSessionByGroupClassId(int id)
        {

            var sessions = await _context.Sessions.Where(x => x.Student.GroupClassId == id).Include(g=>g.Student)
                                                                                            .Include(q=>q.Quiz)
                                                                                            .Include(f => f.Level)
                                                                                            .ThenInclude(z => z.SubjectTheme)
                                                                                                .ThenInclude(a=>a.Subject)
                                                                                                    .AsNoTracking() //"AsNoTracking can greatly speed things up if you're not intending on actually editing the entities and resaving."
                                                                                                        .ToListAsync();

            if (sessions.Count == 0)
                return null;

            var groupedSessionsByStudent = sessions.GroupBy(x => x.StudentId).ToList();

            List<StudentAndCompleteLevelsSessionsDTO> studentsAndCompleteLevels = new();

            foreach(var groupedByStudent in groupedSessionsByStudent)
            {
                //var groupedSessions = groupedByStudent.GroupBy(x => x.Level).ToList();
                var groupedSessions = groupedByStudent.GroupBy(x => x?.Level != null ? $"{x.Level.SubjectTheme.Name}{x.LevelId}" : $"{x?.Quiz?.Name}").ToList();
               
                List<CompleteLevelSessionDTO> completeLevelSessList = new List<CompleteLevelSessionDTO>();

                foreach (var fase in groupedSessions)
                {

                    int? levelId = null;
                    int? quizId = null;
                    LevelDTO level = null;
                    QuizDTO quiz = null;
                    bool? finished;
                    int tries = 0;
                    int repetitions = 0;
                    int fails = 0;
                    int maxScore = 0;
                    int totalScore = 0;
                    float totalElapsedTime = 0;
                    int plays = 0;
                    DateTime lastPlayedDate = new DateTime();


                    

                    if(fase.First().LevelId != null)
                    {
                        levelId = fase.First().LevelId;
                        level = fase.First().Level.Adapt<LevelDTO>();
                        level.Sessions = null; //nao eh necessario o sessions nesse caso, como nao consigo remove-lo no 'Where' ali em cima, removo aqui
                    }
                    if(fase.First().QuizId != null)
                    {
                        quizId = fase.First().QuizId;
                        quiz = fase.First().Quiz.Adapt<QuizDTO>();
                    }

                    finished = fase.Any(x => x.Finished == true) ? true : fase.Any(x => x.Finished == false) ? false : null;
                    fails = fase.Where(x => x.Finished == false).Count();
                    maxScore = fase.Max(x => x.Score);
                    totalScore = fase.Where(x=>x.Finished.HasValue && x.Finished.Value).Sum(x => x.Score);

                    totalElapsedTime = fase.Sum(x => x.ElapsedTime);

                    plays = fase.Count();

                    lastPlayedDate = fase.Max(x => x.PlayedDate);

                    DateTime? minFinishedDate = fase.Min(x => x.FinishedDate);

                    if (minFinishedDate != null)
                    {
                        tries = fase.Where(x => x.PlayedDate < minFinishedDate && x.Finished == false).ToList().Count;
                        repetitions = fase.Where(x => x.FinishedDate > minFinishedDate).ToList().Count;
                    }
                    else
                    {
                        tries = fase.Where(x => x.Finished == false).ToList().Count;
                        repetitions = 0;
                    }


                    completeLevelSessList.Add(new CompleteLevelSessionDTO
                    {
                        LevelId = levelId,
                        Quiz= quiz,
                        QuizId= quizId,
                        Finished = finished,
                        TotalTries = tries,
                        TotalRepetitions = repetitions,
                        TotalFails = fails,
                        MaxScore = maxScore,
                        FinishedDate = minFinishedDate,
                        TotalElapsedTime = totalElapsedTime,
                        TotalPlays = plays,
                        LastPlayedDate = lastPlayedDate,
                        Level = level,
                        TotalScore = totalScore

                    });
                }

                studentsAndCompleteLevels.Add(new StudentAndCompleteLevelsSessionsDTO
                {
                    StudentDto = _context.Students.FirstOrDefault(x=>x.Id == groupedByStudent.Key).Adapt<StudentDTO>(),
                    CompleteLevelSessions = completeLevelSessList.OrderBy(x => x.LevelId).ToList()
                });
            }

            return studentsAndCompleteLevels;
        }

        public async Task<IEnumerable<CompleteStudentSessionDTO>> CompleteStudentsSessionsByTeacherAndGroupClass(TeacherIdAndGroupClassIdDTO dto)
        {
            var sessoes = await _context.Sessions.Where(x=>x.Student.TeacherId == dto.TeacherId && x.Student.GroupClassId == dto.GroupClassId)
                                    .Include(s => s.Student).Include(f => f.Level).Include(q=>q.Quiz).OrderBy(x => x.LevelId).AsNoTracking().ToListAsync();

            if (sessoes.Count == 0)
                return null;

            var groupedSessionsByStudent = sessoes.GroupBy(x => x.StudentId).ToList();

            List<CompleteStudentSessionDTO> completeStudentSessList = new List<CompleteStudentSessionDTO>();
            
            foreach (var studentSessions in groupedSessionsByStudent)
            {
                StudentDTO student = null;
                List<Session> sessionsOfStudent = new();

                foreach (Session session in studentSessions)
                {
                    student = session.Student.Adapt<StudentDTO>();
                    sessionsOfStudent.Add(session);
                }

                var finishedSessions = sessionsOfStudent.Where(x => x.Finished == true).ToList();
                var unfinishedSessions = sessionsOfStudent.Where(x => x.Finished == false).ToList(); //falha somente se finished for false

                DateTime? allLevelsMinimumFinishedDate;
                DateTime? allLevelsMaxFinishedDate;

                if (finishedSessions.Count > 0)
                {
                    allLevelsMinimumFinishedDate = sessionsOfStudent.Where(x => x.Finished == true).OrderByDescending(x => x.FinishedDate).LastOrDefault().FinishedDate;
                    allLevelsMaxFinishedDate = sessionsOfStudent.Where(x => x.Finished == true).OrderByDescending(x => x.FinishedDate).FirstOrDefault().FinishedDate;
                }
                else
                {
                    allLevelsMinimumFinishedDate = null;
                    allLevelsMaxFinishedDate = null;
                }

                #region Score
                Session levelMaxScore;
                var scoreList = sessionsOfStudent.Where(x=>x.Score > 0).OrderByDescending(x => x.Score).ToList();
                if (scoreList.Count() > 0)
                {
                    levelMaxScore = scoreList.FirstOrDefault();
                }
                else
                {
                    levelMaxScore = null;
                }
                #endregion

                #region Repetitions

                /*********mesmo se abrir a fase e apertar o botão de sair, conta como repetição*************/

                int maxRepetitions;
                int totalRepetitions;
                Session levelMaxRepetitions;

                //var replayedSessionsLevelsGroup = sessionsOfStudent.Where(x => x.Finished == true).GroupBy(x => x.LevelId);
                var replayedSessionsLevelsGroup = sessionsOfStudent.Where(x => x.LevelId != null).GroupBy(x => x.LevelId);
                var replayedSessionsQuizGroup = sessionsOfStudent.Where(x => x.QuizId != null).GroupBy(x => x.QuizId);


                List<(int, Session)> levelReplaysTupleList = new();
                List<(int, Session)> quizReplaysTupleList = new();

                foreach (var levels in replayedSessionsLevelsGroup)
                {
                    if (levels.Key == null)
                        continue;

                    DateTime? levelMinFinishedDate = levels.Min(x => x.FinishedDate);
                    int replays;

                    var replayedSessions = levels.Where(x => x.FinishedDate > levelMinFinishedDate).ToList();

                    if (levelMinFinishedDate != null && replayedSessions.Count > 0)
                    {
                        replays = levels.Where(x => x.FinishedDate > levelMinFinishedDate).GroupBy(x => x.LevelId).OrderByDescending(g => g.Count()).FirstOrDefault().Count();

                        levelReplaysTupleList.Add((replays, levels.FirstOrDefault()));
                    }
                    else
                    {
                        replays = 0;
                        levelReplaysTupleList.Add((replays, null));
                    }
                }

                foreach (var quiz in replayedSessionsQuizGroup)
                {
                    if (quiz.Key == null)
                        continue;

                    DateTime? quizMinFinishedDate = quiz.Min(x => x.FinishedDate);
                    int replays;

                    var replayedSessions = quiz.Where(x => x.FinishedDate > quizMinFinishedDate).ToList();

                    if (quizMinFinishedDate != null && replayedSessions.Count > 0)
                    {
                        replays = quiz.Where(x => x.FinishedDate > quizMinFinishedDate).GroupBy(x => x.LevelId).OrderByDescending(g => g.Count()).FirstOrDefault().Count();

                        quizReplaysTupleList.Add((replays, quiz.FirstOrDefault()));
                    }
                    else
                    {
                        replays = 0;
                        quizReplaysTupleList.Add((replays, null));
                    }
                }

                var levelsTotalReplaysCount = levelReplaysTupleList.Sum(x => x.Item1);
                var quizesTotalReplaysCount = quizReplaysTupleList.Sum(x => x.Item1);

                if (levelsTotalReplaysCount > 0 || quizesTotalReplaysCount > 0)
                {
                    if (levelsTotalReplaysCount > quizesTotalReplaysCount)
                    {
                        maxRepetitions = levelReplaysTupleList.Max(x => x.Item1);
                        totalRepetitions = levelReplaysTupleList.Sum(x => x.Item1);
                        levelMaxRepetitions = levelReplaysTupleList.FirstOrDefault(x => x.Item1 == maxRepetitions).Item2;
                    }
                    else
                    {
                        maxRepetitions = quizReplaysTupleList.Max(x => x.Item1);
                        totalRepetitions = quizReplaysTupleList.Sum(x => x.Item1);
                        levelMaxRepetitions = quizReplaysTupleList.FirstOrDefault(x => x.Item1 == maxRepetitions).Item2;
                    }
                }
                else
                {
                    maxRepetitions = 0;
                    totalRepetitions = 0;
                    levelMaxRepetitions = null;
                }

                #endregion

                #region Tries

                /***************mesmo se abrir a fase e apertar o botão de sair, conta como tentativa****************/

                int maxTries;
                int totalTries;
                Session levelMaxTries;

                var triesSessionsLevelsGroup = sessionsOfStudent.Where(x=> x.LevelId != null).GroupBy(x => x.LevelId);
                var triesSessionsQuizGroup = sessionsOfStudent.Where(x=> x.QuizId != null).GroupBy(x => x.QuizId);


                //tries,levelmaxTries
                List<(int,Session)> levelsTriesTupleList = new();
                //tries,quizMaxTries
                List<(int, Session)> quizesTriesTupleList = new();

                foreach (var levels in triesSessionsLevelsGroup) //levels
                {
                    if (levels.Key == null)
                        continue;

                    DateTime? minimumLevelFinishedDate = levels.Min(x=>x.FinishedDate);

                    int tries;

                    if(levels.Where(x=> x.PlayedDate < minimumLevelFinishedDate).Count() > 0)
                    {
                        if (minimumLevelFinishedDate != null)
                        {
                            tries = levels.Where(x => x.PlayedDate < minimumLevelFinishedDate).GroupBy(x => x.LevelId).OrderByDescending(g => g.Count()).FirstOrDefault().Count();

                            levelsTriesTupleList.Add((tries, levels.FirstOrDefault()));
                        }
                        else
                        {
                            tries = levels.Where(x => x.Finished == false).ToList().Count;
                            levelsTriesTupleList.Add((tries, levels.FirstOrDefault()));
                        }
                    }
                    else
                    {
                        tries = 0;
                        levelsTriesTupleList.Add((tries, levels.FirstOrDefault()));
                    }
                    
                }

                foreach (var quizes in triesSessionsQuizGroup) //quizes
                {
                    if (quizes.Key == null)
                        continue;

                    DateTime? minimumQuizFinishedDate = quizes.Min(x=>x.FinishedDate);

                    int tries;

                    if (quizes.Where(x => x.PlayedDate < minimumQuizFinishedDate).Count() > 0)
                    {
                        if (minimumQuizFinishedDate != null)
                        {

                            tries = quizes.Where(x => x.PlayedDate < minimumQuizFinishedDate).GroupBy(x => x.QuizId).OrderByDescending(g => g.Count()).FirstOrDefault().Count();
                            quizesTriesTupleList.Add((tries, quizes.FirstOrDefault()));

                        }
                        else
                        {
                            tries = quizes.Where(x => x.Finished == false).ToList().Count;
                            quizesTriesTupleList.Add((tries, quizes.FirstOrDefault()));
                        }
                    }
                    else
                    {
                        tries = 0;
                        levelsTriesTupleList.Add((tries, quizes.FirstOrDefault()));
                    }
                }

                var levelsTotalTriesCount = levelsTriesTupleList.Sum(x => x.Item1);
                var quizesTotalTriesCount = quizesTriesTupleList.Sum(x => x.Item1);

                if (levelsTotalTriesCount > 0 || quizesTotalTriesCount > 0)
                {
                    if(levelsTotalTriesCount > quizesTotalTriesCount)
                    {
                        maxTries = levelsTriesTupleList.Max(x => x.Item1);
                        totalTries = levelsTriesTupleList.Sum(x => x.Item1);
                        levelMaxTries = levelsTriesTupleList.FirstOrDefault(x => x.Item1 == maxTries).Item2;
                    }
                    else
                    {
                        maxTries = quizesTriesTupleList.Max(x => x.Item1);
                        totalTries = quizesTriesTupleList.Sum(x => x.Item1);
                        levelMaxTries = quizesTriesTupleList.FirstOrDefault(x => x.Item1 == maxTries).Item2;
                    }                    
                }
                else
                {
                    maxTries = 0;
                    totalTries = 0;
                    levelMaxTries = null;
                }
                

                #endregion

                #region Fails

                int maxFails;
                int totalFails;
                Session levelMaxFail;

                if (unfinishedSessions.Count() > 0)
                {
                    maxFails = unfinishedSessions.GroupBy(x => x.LevelId).OrderByDescending(g => g.Count()).FirstOrDefault().Count();
                    totalFails = unfinishedSessions.ToList().Count();

                    levelMaxFail = unfinishedSessions.GroupBy(x => x.LevelId).OrderByDescending(g => g.Count()).FirstOrDefault().Select(x => x).FirstOrDefault();
                }
                else
                {
                    levelMaxFail = null;
                    maxFails = 0;
                    totalFails = 0;
                }
                

                #endregion

                #region ElapsedTime

                var levelMaxElapsedTime = sessionsOfStudent.Where(x=>x.ElapsedTime > 0).OrderByDescending(x => x.ElapsedTime).FirstOrDefault();

                #endregion

                #region finishedLevelsQuantity
                int finishedLevelsQuantity;
                int finishedQuizesQuantity;

                if (finishedSessions.Count > 0)
                {
                    finishedLevelsQuantity = finishedSessions.GroupBy(p => p.LevelId).Select(g => g.FirstOrDefault()).ToList().Count(); //separando em grupos no caso o primeiro grupo faz uma separacao distinta, (tipo o distinct so que se baseando em propriedades), como fnciona eu nao sei LOL
                }
                else
                {
                    finishedLevelsQuantity = 0;
                }

                #endregion

                #region totalLevelsQuantity
                var studentGroupClassSubjectThemes = _context.GroupClassSubjectThemes.Where(x => x.GroupClassId == student.GroupClassId)
                                                                                .Include(x=>x.SubjectTheme).ThenInclude(x=>x.Levels).AsNoTracking().ToList();

                var studentActiveQuizes = _context.Quiz.Where(x => x.TeacherId == student.TeacherId && x.Active).ToList();

                List<SubjectTheme> subjectThemes = new();

                foreach(GroupClassSubjectTheme subj in studentGroupClassSubjectThemes)
                {
                    subjectThemes.Add(subj.SubjectTheme);    
                }

                int totalLevelsQuantity = 0;
                foreach(var subjTheme in subjectThemes)
                {
                    totalLevelsQuantity += subjTheme.Levels.Count;
                }

                totalLevelsQuantity += studentActiveQuizes.Count;

                #endregion

                #region Plays
                int maxPlays;
                int totalPlays;
                Session LevelMaxPlays;

                totalPlays = sessionsOfStudent.Count;

                //var levelsPlays = sessionsOfStudent.GroupBy(x => x.LevelId).OrderByDescending(g => g.Count()).FirstOrDefault().Select(x => x).FirstOrDefault().LevelId;

                var allLevelsPlays = sessionsOfStudent.Where(x => x.LevelId != null).GroupBy(x => x.LevelId);
                var allQuizesPlays = sessionsOfStudent.Where(x => x.QuizId!= null).GroupBy(x => x.QuizId);

                List<(int, Session)> levelPlaysTupleList = new();
                List<(int, Session)> quizPlaysTupleList = new();

                foreach (var levels in allLevelsPlays)
                {
                    if (levels.Key == null)
                        continue;
                    
                    levelPlaysTupleList.Add((levels.Count(), levels.FirstOrDefault()));
                }

                foreach (var quiz in allQuizesPlays)
                {
                    if (quiz.Key == null)
                        continue;

                    quizPlaysTupleList.Add((quiz.Count(), quiz.FirstOrDefault()));
                }

                var levelsPlaysCount = levelPlaysTupleList.Sum(x => x.Item1);
                var quizesPlaysCount = quizPlaysTupleList.Sum(x => x.Item1);

                if (levelsPlaysCount > 0 || quizesPlaysCount > 0)
                {
                    if (levelsPlaysCount > quizesPlaysCount)
                    {
                        maxPlays = levelPlaysTupleList.Max(x => x.Item1);
                        LevelMaxPlays = levelPlaysTupleList.FirstOrDefault(x => x.Item1 == maxPlays).Item2;
                    }
                    else
                    {
                        maxPlays = quizPlaysTupleList.Max(x => x.Item1);
                        LevelMaxPlays = quizPlaysTupleList.FirstOrDefault(x => x.Item1 == maxPlays).Item2;
                    }
                }
                else
                {
                    maxPlays = 0;
                    LevelMaxPlays = null;
                }


                #endregion

                #region dEBUG
                // ################# DEBUG PURPOSES ########################
                /* var StudentId = sessionsOfStudent[0].StudentId;
                 var MaxScore = sessionsOfStudent.Max(x => x.Score);
                 var TotalScore = sessionsOfStudent.Sum(x => x.Score);
                 int? LevelMaxScoreId = levelMaxScore == null ? null : levelMaxScore.LevelId;
                 var MaxRepetitions = maxRepetitions;
                 var TotalRepetitions = totalRepetitions;
                 int? LevelMaxRepetitionsId = levelMaxRepetitions == null ? null : levelMaxRepetitions.LevelId;
                 var MaxTries = maxTries;
                 var TotalTries = totalTries;
                 int? LevelMaxTriesId = levelMaxTries == null ? null : levelMaxTries.Id;
                 var MaxFails = maxFails;
                 var TotalFails = totalFails;
                 int? LevelMaxFailsId = levelMaxFail == null ? null : levelMaxFail.Id;
                 var MaxElapsedTime = sessionsOfStudent.Max(x => x.ElapsedTime);
                 var TotalElapsedTime = sessionsOfStudent.Sum(x => x.ElapsedTime);
                 int? LevelMaxElapsedTimeId = levelMaxElapsedTime == null ? null : levelMaxElapsedTime.Id;
                 var MinFinishedDate = miniFinishedDate;
                 var MaxFinishedDate = maxFinishedDate;
                 var LevelsFinishedQuantity = finishedLevelsQuantity;
                 var LevelsPendingQuantity = totalLevelsQuantity - finishedLevelsQuantity;
                 var MaxPlays = sessionsOfStudent.GroupBy(x => x.LevelId).OrderByDescending(g => g.Count()).FirstOrDefault().Select(x => x).Count();
                 var TotalPlays = sessionsOfStudent.Count;
                 var LevelMaxPlays = sessionsOfStudent.GroupBy(x => x.LevelId).OrderByDescending(g => g.Count()).FirstOrDefault().Select(x => x).FirstOrDefault().LevelId;
                 var LastPlayedDate = sessionsOfStudent.Max(x => x.PlayedDate);
                 var LastLoginDate = sessionsOfStudent.FirstOrDefault().Student.LastLoginDate;*/
                #endregion
                completeStudentSessList.Add(new CompleteStudentSessionDTO
                {
                    Student = student,
                    StudentId = sessionsOfStudent[0].StudentId,
                    MaxScore = sessionsOfStudent.Max(x => x.Score),
                    TotalScore = sessionsOfStudent.Where(x=>x.Finished.HasValue && x.Finished.Value).Sum(x => x.Score),
                    LevelMaxScore = levelMaxScore.Adapt<SessionDTO>(),
                    MaxRepetitions = maxRepetitions,
                    TotalRepetitions = totalRepetitions,
                    LevelMaxRepetitions = levelMaxRepetitions.Adapt<SessionDTO>(), /*mesmo se abrir a fase e apertar o botão de sair, conta como repetição*/
                    MaxTries = maxTries,
                    TotalTries = totalTries,
                    LevelMaxTries = levelMaxTries.Adapt<SessionDTO>(), /*mesmo se abrir a fase e apertar o botão de sair, conta como tentativa*/
                    MaxFails = maxFails,
                    TotalFails = totalFails,
                    LevelMaxFails = levelMaxFail.Adapt<SessionDTO>(),
                    MaxElapsedTime = sessionsOfStudent.Max(x => x.ElapsedTime),
                    TotalElapsedTime = sessionsOfStudent.Sum(x => x.ElapsedTime),
                    LevelMaxElapsedTime = levelMaxElapsedTime.Adapt<SessionDTO>(),
                    MinFinishedDate = allLevelsMinimumFinishedDate,
                    MaxFinishedDate = allLevelsMaxFinishedDate,
                    LevelsFinishedQuantity = finishedLevelsQuantity,
                    LevelsPendingQuantity = totalLevelsQuantity - finishedLevelsQuantity,
                    MaxPlays = maxPlays,
                    TotalPlays = totalPlays,
                    LevelMaxPlays = LevelMaxPlays.Adapt<SessionDTO>(),
                    LastPlayedDate = sessionsOfStudent.Max(x => x.PlayedDate),
                    LastLoginDate = sessionsOfStudent.FirstOrDefault().Student.LastLoginDate,
                }) ; 
            }

            return completeStudentSessList;
        }
        
        public async Task<IEnumerable<Session>> GetHighScoreByLevelIdOrQuizIdAndGroupClass(GroupClassLevelIdAndQuizIdDTO obj)
        {
            List<Session> sessions = new();
            if(obj.LevelId != null)
            {
                sessions = await _context.Sessions.Where(x => x.Finished.Value && x.LevelId != null && x.LevelId == obj.LevelId && x.Student.GroupClassId == obj.GroupClassId && x.Score > 0)
                                                    .OrderByDescending(x => x.Score).Include(x => x.Student).ToListAsync();
            }
            if(obj.QuizId != null)
            {
                sessions = await _context.Sessions.Where(x => x.Finished.Value && x.QuizId != null && x.QuizId == obj.QuizId && x.Student.GroupClassId == obj.GroupClassId && x.Score > 0)
                                                    .OrderByDescending(x => x.Score).Include(x => x.Student).ToListAsync();
            }

            if (sessions.Count == 0)
                return null;

            var groupedSessionsByStudent = sessions.GroupBy(x => x.StudentId).ToList();


            List<Session> topScoresList = new();

            int count = 0;

            foreach (var studentSessions in groupedSessionsByStudent)
            {
                if (count >= 5) break; //pegando somente o top 5

                var studentMaxScore = studentSessions.OrderByDescending(x => x.Score).First();
                topScoresList.Add(studentMaxScore);
                count++;
            }
            
            return topScoresList;
        }

        public async Task<IEnumerable<StudentAndScoreDTO>> GetHighScoresOfGroupClass(int groupClassId)
        {
            var sessions = await _context.Sessions.Where(x=> x.Finished.Value && x.Student.GroupClassId == groupClassId && x.Score > 0).Include(x => x.Student).AsNoTracking().ToListAsync();

            if (sessions.Count == 0)
                return null;

            var groupedSessionsByStudent = sessions.GroupBy(x => x.StudentId).ToList();

            List<StudentAndScoreDTO> topScoresList = new();

            foreach (var studentSessions in groupedSessionsByStudent)
            {

                var student = studentSessions.FirstOrDefault().Student;
                var studentTotalScore = studentSessions.Sum(x => x.Score);

                topScoresList.Add(new StudentAndScoreDTO
                {
                    StudentId = student.Id,
                    Student = student.Adapt<StudentDTO>(),
                    Score = studentTotalScore
                });

            }

            return topScoresList.OrderByDescending(x=>x.Score).Take(5);
        }

        public async Task<Session> GetStudentTopScoreByLevel(StudentAndLevelIdDTO obj)
        {
            var sessions = await _context.Sessions.Where(x => x.StudentId == obj.StudentId && x.LevelId == obj.LevelId && x.Score > 0).ToListAsync();

            if (sessions.Count == 0)
                return null;

            return sessions.OrderByDescending(x => x.Score).First();
        }

        public async Task<IEnumerable<Session>> GetStudentLevelsTopScores(int id)
        {
            var sessions = await _context.Sessions.Where(x => x.StudentId == id && x.Finished.Value && x.Score > 0 && x.LevelId != null).ToListAsync();

            if (sessions.Count == 0)
                return null;

            var groupedSessions = sessions.GroupBy(x => x.LevelId);

            List<Session> maxScoreLevelsSessions = new();

            foreach(var levelSessions in groupedSessions)
            {
                maxScoreLevelsSessions.Add(levelSessions.OrderByDescending(x => x.Score).First());
            }

            return maxScoreLevelsSessions;
        }

        public async Task<IEnumerable<Session>> GetStudentQuizesTopScores(int id)
        {
            var sessions = await _context.Sessions.Where(x => x.StudentId == id && x.Finished.Value && x.Score > 0 && x.QuizId != null).ToListAsync();

            if (sessions.Count == 0)
                return null;

            var groupedSessions = sessions.GroupBy(x => x.QuizId);

            List<Session> maxScoreLevelsSessions = new();

            foreach (var levelSessions in groupedSessions)
            {
                maxScoreLevelsSessions.Add(levelSessions.OrderByDescending(x => x.Score).First());
            }

            return maxScoreLevelsSessions;
        }

        public async Task<Session> CreateSession(SessionDTO dto)
        {
            var session = dto.Adapt<Session>();

            _context.Sessions.Add(session);

            await _context.SaveChangesAsync();

            return session;
        }
    }
}
