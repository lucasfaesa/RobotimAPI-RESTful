using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Mestrado_Lucas.Context;
using API_Mestrado_Lucas.Models;
using Mapster;

namespace API_Mestrado_Lucas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QuizesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Quizes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizDTO>>> GetQuizes()
        {
            return _context.Quiz.ToListAsync().Adapt<List<QuizDTO>>();
        }

        [HttpGet("GetTeacherQuizes/{id}")]
        public async Task<ActionResult<IEnumerable<QuizDTO>>> GetTeacherQuizes(int id)
        {
            var quizes = await _context.Quiz.Where(x => x.TeacherId == id).Include(x=>x.Teacher).Include(X=> X.Questions).ThenInclude(X=>X.QuestionAnswers).AsNoTracking().ToListAsync();

            if(quizes.Count == 0)
                return new List<QuizDTO>();

            return quizes.Adapt<List<QuizDTO>>();
        }

        [HttpGet("GetTeacherActiveQuizes/{id}")]
        public async Task<ActionResult<IEnumerable<QuizDTO>>> GetTeacherActiveQuizes(int id)
        {
            var quizes = await _context.Quiz.Where(x => x.TeacherId == id && x.Active).Include(x => x.Teacher).Include(X => X.Questions).ThenInclude(X => X.QuestionAnswers).AsNoTracking().ToListAsync();

            if (quizes.Count == 0)
                return new List<QuizDTO>();

            return quizes.Adapt<List<QuizDTO>>();
        }

        // GET: api/Quizes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuizDTO>> GetQuiz(int id)
        {
            var quiz = await _context.Quiz.Where(x=>x.Id == id).Include(x=>x.Questions).ThenInclude(x=>x.QuestionAnswers).AsNoTracking().FirstOrDefaultAsync();

            if (quiz == null)
            {
                return NotFound();
            }

            return quiz.Adapt<QuizDTO>();
        }

        // PUT: api/Quizes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuiz(int id, Quiz quiz)
        {
            if (id != quiz.Id)
            {
                return BadRequest();
            }

            _context.Entry(quiz).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizExists(id))
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

        // POST: api/Quizes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<QuizDTO>> PostQuiz(QuizDTO quiz)
        {
            var oldQuiz = _context.Quiz.Where(x => x.Id == quiz.Id && x.TeacherId == quiz.TeacherId).Include(x=>x.Questions).FirstOrDefault();

            //removendo quiz antigo
            if (oldQuiz != null)
            {
                oldQuiz.Name = quiz.Name.Trim();
                oldQuiz.Questions = quiz.Adapt<Quiz>().Questions;
                //_context.Quiz.Remove(oldQuiz);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                var quizz = new Quiz()
                {
                    Name = quiz.Name.Trim(),
                    TeacherId = quiz.TeacherId,
                    Questions = quiz.Adapt<Quiz>().Questions
                };

                _context.Quiz.Add(quizz);
                await _context.SaveChangesAsync();

                return Ok();
            }

        }

        [HttpPost("ChangeActiveQuizes/")]
        public async Task<ActionResult<QuizDTO>> ChangeActiveQuizes(List<QuizDTO> quizes)
        {
            if (quizes.Count == 0) return BadRequest();

            var quizesConverted = quizes.Adapt<List<Quiz>>();

            foreach( var quiz in quizesConverted){

                if (!_context.Quiz.Any(x=>x.Id == quiz.Id))
                {
                    return NotFound();
                }

                _context.Entry(quiz).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
           
            return NoContent();

        }

        [HttpPost("ChangeActiveQuiz/")]
        public async Task<ActionResult<QuizDTO>> ChangeActiveQuizes(QuizDTO quiz)
        {
            if (quiz == null) return BadRequest();

            var quizConverted = quiz.Adapt<Quiz>();

            if (!_context.Quiz.Any(x => x.Id == quiz.Id))
            {
                return NotFound();
            }

            _context.Entry(quizConverted).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();

        }



        [HttpPost("BatchDeleteQuizes/")]
        public async Task<ActionResult<ICollection<QuizDTO>>> BatchDelete(List<QuizDTO> quizes)
        {
            List<Quiz> quizesToBeRemoved = new();

            foreach (var quiz in quizes)
            {
                if (quiz.TeacherId == null)
                    continue;

                quizesToBeRemoved.Add(await _context.Quiz.FindAsync(quiz.Id));
            }

            if (quizesToBeRemoved.Count == 0)
            {
                return NoContent();
            }

            _context.Quiz.RemoveRange(quizesToBeRemoved);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Quizes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            var quiz = await _context.Quiz.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            _context.Quiz.Remove(quiz);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuizExists(int id)
        {
            return _context.Quiz.Any(e => e.Id == id);
        }
    }
}
