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
    public class QuestionAnswersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public QuestionAnswersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/QuestionAnswers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionAnswerDTO>>> GetQuestionAnswer()
        {
            var questions = await _context.QuestionAnswer.Include(x=>x.Question).AsNoTracking().ToListAsync();
            return questions.Adapt<List<QuestionAnswerDTO>>();
        }

        // GET: api/QuestionAnswers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionAnswerDTO>> GetQuestionAnswer(int id)
        {
            var questionAnswer = await _context.QuestionAnswer.Where(x=>x.Id == id).Include(x=>x.Question).AsNoTracking().FirstOrDefaultAsync();

            if (questionAnswer == null)
            {
                return NotFound();
            }

            return questionAnswer.Adapt<QuestionAnswerDTO>();
        }

        // PUT: api/QuestionAnswers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestionAnswer(int id, QuestionAnswer questionAnswer)
        {
            if (id != questionAnswer.Id)
            {
                return BadRequest();
            }

            _context.Entry(questionAnswer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionAnswerExists(id))
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

        // POST: api/QuestionAnswers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<QuestionAnswerDTO>> PostQuestionAnswer(QuestionAnswerDTO questionAnswer)
        {
            _context.QuestionAnswer.Add(questionAnswer.Adapt<QuestionAnswer>());
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestionAnswer", new { id = questionAnswer.Id }, questionAnswer);
        }

        // POST: api/QuestionAnswers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("BulkPostQuestionAnswers/")]
        public async Task<ActionResult<QuestionAnswerDTO>> BulkPostQuestionAnswers(ICollection<QuestionAnswerDTO> questionAnswers)
        {

            _context.QuestionAnswer.AddRange(questionAnswers.Adapt<List<QuestionAnswer>>());
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/QuestionAnswers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestionAnswer(int id)
        {
            var questionAnswer = await _context.QuestionAnswer.FindAsync(id);
            if (questionAnswer == null)
            {
                return NotFound();
            }

            _context.QuestionAnswer.Remove(questionAnswer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuestionAnswerExists(int id)
        {
            return _context.QuestionAnswer.Any(e => e.Id == id);
        }
    }
}
