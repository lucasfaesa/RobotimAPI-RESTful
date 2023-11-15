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
    public class QuestionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QuestionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Questions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDTO>>> GetQuestions()
        {
            var questions = await _context.Question.Include(x => x.QuestionAnswers).AsNoTracking().ToListAsync();

            return questions.Adapt<List<QuestionDTO>>();
        }

        // GET: api/Questions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDTO>> GetQuestion(int id)
        {
            var question = await _context.Question.Where(x=>x.Id == id).Include(x => x.QuestionAnswers).AsNoTracking().FirstOrDefaultAsync();

            if (question == null)
            {
                return NotFound();
            }

            return question.Adapt<QuestionDTO>();
        }

        // GET: api/Questions
        [HttpGet("GetQuestionsByQuizId/{id}")]
        public async Task<ActionResult<IEnumerable<QuestionDTO>>> GetQuestionsByQuizId(int id)
        {
            var questions = await _context.Question.Where(x => x.QuizId == id).Include(x=>x.QuestionAnswers).Include(x=>x.Quiz).AsNoTracking().ToListAsync();

            return questions.Adapt<List<QuestionDTO>>();
        }

        /*[HttpGet("GetQuestionsByQuizId/{id}")]
        public async Task<ActionResult<IEnumerable<QuestionDTO>>> GetQuestionsByQuizId(int id)
        {
            var defaultQuestions = await _context.Question.Where(x => x.TeacherId == null).Include(x => x.QuestionAnswers).Include(x => x.Subject).AsNoTracking().ToListAsync();

            var teacherQuestions = await _context.Question.Where(x=>x.TeacherId == id).Include(x => x.QuestionAnswers).Include(x => x.Subject).AsNoTracking().ToListAsync();

            List<Question> questions = new(defaultQuestions.Adapt<List<Question>>());
            
            if(teacherQuestions.Count > 0)
            {
                var questionsBySubject = questions.GroupBy(x => x.SubjectId);

                foreach(var group in questionsBySubject)
                {
                    if(teacherQuestions.Any(x=>x.SubjectId == group.Key))
                    {
                        questions = questions.Where(x => x.SubjectId != group.Key).ToList();
                    }
                }
                questions.AddRange(teacherQuestions);
            }

            return questions.OrderBy(x=>x.SubjectId).Adapt<List<QuestionDTO>>();
        }*/


        // PUT: api/Questions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestion(int id, Question question)
        {
            if (id != question.Id)
            {
                return BadRequest();
            }

            _context.Entry(question).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
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

        // POST: api/Questions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<QuestionDTO>> PostQuestion(QuestionDTO question)
        {
            _context.Question.Add(question.Adapt<Question>());

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestion", new { id = question.Id }, question);
        }

        [HttpPost("BulkPostQuestions/")]
        public async Task<ActionResult<QuestionDTO>> BulkPostQuestions(ICollection<QuestionDTO> questions)
        {
            var questionsList = questions.ToList();

            if(questionsList[0].Quiz.Teacher != null) //evitando de substituir o quiz default
            {
                //removendo perguntas e respostas antigas
                var oldQuestions = _context.Question.Where(x => x.QuizId == questionsList[0].QuizId).ToList();

                if(oldQuestions.Count > 0)
                    _context.Question.RemoveRange(oldQuestions);
            }

            _context.Question.AddRange(questionsList.Adapt<List<Question>>());

            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Questions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _context.Question.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            _context.Question.Remove(question);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuestionExists(int id)
        {
            return _context.Question.Any(e => e.Id == id);
        }
    }
}
