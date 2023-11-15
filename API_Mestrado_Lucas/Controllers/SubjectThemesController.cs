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
    public class SubjectThemesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SubjectThemesController(AppDbContext context)
        {
            _context = context;
        }


        // GET: api/SubjectThemes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubjectThemeDTO>> GetSubjectTheme(int id)
        {
            var subjectTheme = await _context.SubjectThemes.Where(x=>x.Id == id).Include(x=>x.Subject).AsNoTracking().FirstOrDefaultAsync();


            if (subjectTheme == null)
            {
                return NotFound();
            }

            return subjectTheme.Adapt<SubjectThemeDTO>();
        }

        // GET: api/SubjectThemes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjectThemeDTO>>> GetSubjectThemes()
        {
            var subjectThemes = await _context.SubjectThemes.Include(x => x.Subject).AsNoTracking().ToListAsync();

            return subjectThemes.Adapt<List<SubjectThemeDTO>>();
        }

        [HttpGet("GetSubjectThemeByCode/{code}")]
        public async Task<ActionResult<SubjectThemeDTO>> GetThemeByCode(string code)
        {
            var subjectTheme = await _context.SubjectThemes.Where(x => x.Code == code).AsNoTracking().FirstOrDefaultAsync();

            if (subjectTheme == null)
            {
                return NotFound();
            }

            return subjectTheme.Adapt<SubjectThemeDTO>();
        }

        // PUT: api/SubjectThemes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubjectTheme(int id, SubjectTheme subjectTheme)
        {
            if (id != subjectTheme.Id)
            {
                return BadRequest();
            }

            _context.Entry(subjectTheme).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectThemeExists(id))
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

        // POST: api/SubjectThemes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SubjectThemeDTO>> PostSubjectTheme(SubjectThemeDTO subjectTheme)
        {
            _context.SubjectThemes.Add(subjectTheme.Adapt<SubjectTheme>());
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubjectTheme", new { id = subjectTheme.Id }, subjectTheme);
        }

        // DELETE: api/SubjectThemes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubjectTheme(int id)
        {
            var subjectTheme = await _context.SubjectThemes.FindAsync(id);
            if (subjectTheme == null)
            {
                return NotFound();
            }

            _context.SubjectThemes.Remove(subjectTheme);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubjectThemeExists(int id)
        {
            return _context.SubjectThemes.Any(e => e.Id == id);
        }
    }
}
