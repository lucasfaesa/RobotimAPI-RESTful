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
    public class StudentWrongAnswersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentWrongAnswersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/StudentWrongAnswers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentWrongAnswers>>> GetAllStudentsWrongAnswers()
        {
            return await _context.StudentWrongAnswers.ToListAsync();
        }

        // GET: api/StudentWrongAnswers/5
        [HttpGet("{studentId}")]
        public async Task<ActionResult<IEnumerable<StudentWrongAnswers>>> GetStudentWrongAnswers(int studentId)
        {
            var studentWrongAnswers = await _context.StudentWrongAnswers.Where(x => x.Id == studentId).Include(x => x.Student).AsNoTracking().ToListAsync();

            if (studentWrongAnswers == null)
            {
                return NotFound();
            }

            return studentWrongAnswers;
        }

        // GET: api/StudentWrongAnswers/GroupClassWrongAnswers/5
        [HttpGet("GroupClassWrongAnswers/{groupClassId}")]
        public async Task<ActionResult<IEnumerable<StudentWrongAnswers>>> GroupClassWrongAnswers(int groupClassId)
        {
            var studentWrongAnswers = await _context.StudentWrongAnswers.Where(x => x.Student.GroupClassId == groupClassId).AsNoTracking().ToListAsync();

            if (studentWrongAnswers == null)
            {
                return NotFound();
            }

            return studentWrongAnswers;
        }

        // PUT: api/StudentWrongAnswers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentWrongAnswers(int id, StudentWrongAnswers studentWrongAnswers)
        {
            if (id != studentWrongAnswers.Id)
            {
                return BadRequest();
            }

            _context.Entry(studentWrongAnswers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentWrongAnswersExists(id))
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

        // POST: api/StudentWrongAnswers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudentWrongAnswers>> PostStudentWrongAnswers(StudentWrongAnswers studentWrongAnswers)
        {
            _context.StudentWrongAnswers.Add(studentWrongAnswers);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentWrongAnswers", new { id = studentWrongAnswers.Id }, studentWrongAnswers);
        }

        [HttpPost("BulkPostStudentsWrongAnswers/")]
        public async Task<ActionResult<StudentWrongAnswers>> BulkPostStudentsWrongAnswers(ICollection<StudentWrongAnswersDTO> answers)
        {
            _context.StudentWrongAnswers.AddRange(answers.Adapt<List<StudentWrongAnswers>>());

            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/StudentWrongAnswers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentWrongAnswers(int id)
        {
            var studentWrongAnswers = await _context.StudentWrongAnswers.FindAsync(id);
            if (studentWrongAnswers == null)
            {
                return NotFound();
            }

            _context.StudentWrongAnswers.Remove(studentWrongAnswers);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentWrongAnswersExists(int id)
        {
            return _context.StudentWrongAnswers.Any(e => e.Id == id);
        }
    }
}
