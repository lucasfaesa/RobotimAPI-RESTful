using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Mestrado_Lucas.Context;
using API_Mestrado_Lucas.Models;
using API_Mestrado_Lucas.Services.Interfaces;
using Mapster;

namespace API_Mestrado_Lucas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ISessionService _sessionService;
        public SessionsController(AppDbContext context, ISessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
        }

        // GET: api/Sessions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionDTO>>> GetSessions()
        {
            var sessions = await _context.Sessions.ToListAsync();

            var sessionDTO = sessions.Adapt<List<SessionDTO>>();

            return sessionDTO;
        }

        // GET: api/Sessions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SessionDTO>> GetSession(int id)
        {
            var session = await _context.Sessions.FindAsync(id);

            if (session == null)
            {
                return NotFound();
            }

            var sessionDto = session.Adapt<SessionDTO>();

            return sessionDto;
        }

        [HttpGet("GetStudentSessions/{id}")]
        public async Task<ActionResult<IEnumerable<SessionDTO>>> GetStudentSessions(int id)
        {
            var session = await _context.Sessions.Where(x => x.StudentId == id).Include(f => f.Level)
                                                                                .ThenInclude(z=>z.SubjectTheme)
                                                                                .ThenInclude(a=>a.Subject)
                                                                                .AsNoTracking() //"AsNoTracking can greatly speed things up if you're not intending on actually editing the entities and resaving."
                                                                                .ToListAsync();

            if (session == null)
            {
                return NotFound();
            }
          

            var sessionDto = session.Adapt<List<SessionDTO>>();

            return sessionDto;
        }

        // PUT: api/Sessions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSession(int id, SessionDTO session)
        {

            if (id != session.Id)
            {
                return BadRequest();
            }

            _context.Entry(session).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Sessions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SessionDTO>> PostSession(SessionDTO dto)
        {
            try
            {
                var session = await _sessionService.CreateSession(dto);

                Console.WriteLine("Created Session");

                return CreatedAtAction("GetSession", new { id = session.Id }, session);
            }
            catch
            {
                Console.WriteLine("Error when creating session");

                return NotFound("Not possible to save session");
            }
            
        }

        [HttpPost("UpdateSession/")]
        public async Task<ActionResult<SessionDTO>> UpdateSession(SessionDTO dto)
        {
            if (!SessionExists(dto.Id))
            {
                return NotFound("Não encontrada sessão com esse Id");
            }

            var session = await _context.Sessions.FirstOrDefaultAsync(x => x.Id == dto.Id);

            session.ElapsedTime = dto.ElapsedTime;
            session.Finished = dto.Finished;
            session.FinishedDate = dto.FinishedDate;
            session.Score = dto.Score;

            await _context.SaveChangesAsync();

            return session.Adapt<SessionDTO>();
        } 


        // DELETE: api/Sessions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSession(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }

            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SessionExists(int id)
        {
            return _context.Sessions.Any(e => e.Id == id);
        }

        [HttpGet("CompleteStudentsLevelSessionByGroupClassId/{id}")]
        public async Task<ActionResult<IEnumerable<StudentAndCompleteLevelsSessionsDTO>>> CompleteStudentsLevelSessionByGroupClassId(int id)
        {
            var result = await _sessionService.CompleteStudentsLevelSessionByGroupClassId(id);

            if (result == null)
            {
                List<StudentAndCompleteLevelsSessionsDTO> emptyList = new();//retornar assim fica mais facil de lidar na unity (nesse caso) do que um bad request ou no content
                return emptyList;
            }

            return Ok(result);
        }

        [HttpPost("CompleteStudentsSessionsByTeacherAndGroupClass/")]
        public async Task<ActionResult<IEnumerable<CompleteStudentSessionDTO>>> CompleteStudentsSessionsByTeacherAndGroupClass(TeacherIdAndGroupClassIdDTO dto)
        {
            var result = await _sessionService.CompleteStudentsSessionsByTeacherAndGroupClass(dto);

            if (result == null)
            {
                List<CompleteStudentSessionDTO> emptyList = new(); //retornar assim fica mais facil de lidar na unity (nesse caso) do que um bad request ou no content
                return emptyList;
            }

            return Ok(result);
        }


        [HttpPost("HighScoreByLevelOrQuizAndGroupClass/")]
        public async Task<ActionResult<IEnumerable<StudentAndScoreDTO>>> GetHighScoreByLevelIdOrQuizIdAndGroupClass(GroupClassLevelIdAndQuizIdDTO obj)
        {
            var result = await _sessionService.GetHighScoreByLevelIdOrQuizIdAndGroupClass(obj);

            if (result == null)
            {
                return NoContent();
            }

            return Ok(result.Adapt<List<StudentAndScoreDTO>>());
        }

        [HttpGet("HighScoresOfGroupClass/{groupClassId}")]
        public async Task<ActionResult<IEnumerable<StudentAndScoreDTO>>> HighScoresOfGroupClass(int groupClassId)
        {
            var result = await _sessionService.GetHighScoresOfGroupClass(groupClassId);

            if (result == null)
            {
                return NoContent();
            }

            return Ok(result);
        }

        [HttpPost("StudentTopScoreByLevelId/")]
        public async Task<ActionResult<IEnumerable<SessionDTO>>> GetStudentTopScoreByLevel(StudentAndLevelIdDTO obj)
        {
            var result = await _sessionService.GetStudentTopScoreByLevel(obj);

            if (result == null)
            {
                return NoContent();
            }

            return Ok(result.Adapt<SessionDTO>());
        }

        [HttpGet("LevelsTopScoresByStudentId/{id}")]
        public async Task<ActionResult<IEnumerable<SessionDTO>>> GetStudentLevelsTopScores(int id)
        {
            var result = await _sessionService.GetStudentLevelsTopScores(id);

            if (result == null)
            {
                return NoContent();
            }

            return Ok(result.Adapt<List<SessionDTO>>());
        }

        [HttpGet("QuizesTopScoresByStudentId/{id}")]
        public async Task<ActionResult<IEnumerable<SessionDTO>>> GetStudentQuizesTopScores(int id)
        {
            var result = await _sessionService.GetStudentQuizesTopScores(id);

            if (result == null)
            {
                return NoContent();
            }

            return Ok(result.Adapt<List<SessionDTO>>());
        }


        #region debug purposes
        [HttpGet("DataOfLevels/{id}")]
        public async Task<ActionResult<IEnumerable<SessionDTO>>> DataOfLevels(int id)
        {
            var result = await _context.Sessions.Where(x => x.LevelId == id && x.Finished.HasValue && x.Finished.Value).Include(x=>x.Student).AsNoTracking().ToListAsync();

            if (result == null)
            {
                return NoContent();
            }

            return Ok(result.Adapt<List<SessionDTO>>());
        }

        [HttpGet("ScoresOfLevels")]
        public async Task<ActionResult<string>> ScoresOfLevels()
        {
            var result = await _context.Sessions.Where(x => x.Finished.HasValue && x.Finished.Value && x.Score > 0).Include(x => x.Student).Include(x=>x.Level).ThenInclude(x=>x.SubjectTheme).AsNoTracking().ToListAsync();

            if (result == null)
            {
                return NoContent();
            }

            string finalString = "";

            var groupedResult = result.GroupBy(x => x.LevelId);

            foreach(var group in groupedResult)
            {
                string levelName = _context.Levels.Where(x => x.Id == group.Key).Include(x=>x.SubjectTheme).First().SubjectTheme.Name;
                finalString += $"{levelName}[{group.Key}]: ";

                var orderedGroups = group.OrderBy(x => x.Score);

                foreach(var session in orderedGroups)
                {
                    finalString += $"{session.Score}, ";
                }
                finalString+= "\n";
            }

            return Ok(finalString);
        }

        [HttpGet("SpaceshipSessionsOfTeacher/{id}")]
        public async Task<ActionResult<IEnumerable<SessionDTO>>> SpaceshipSessions(int id)
        {
            var result = await _context.Sessions.Where(x => x.Student.TeacherId == id).Include(x => x.Student).Include(x=>x.Quiz).AsNoTracking().ToListAsync();

            result = result.Where(x=>x.QuizId != null).ToList();

            if (result == null)
            {
                return NoContent();
            }

            return Ok(result.Adapt<List<SessionDTO>>());
        }

        #endregion
    }
}
