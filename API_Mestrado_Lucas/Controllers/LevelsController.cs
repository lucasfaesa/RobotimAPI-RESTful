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
    public class LevelsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LevelsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Levels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LevelDTO>>> GetLevels()
        {
            var levels = await _context.Levels.Include(x => x.SubjectTheme).ThenInclude(x => x.Subject)
                                                        .AsNoTracking().ToListAsync(); //AsNoTracking evita que fique puxando as propriedades virtuais sem eu usar o include, ex: o subjectTheme tem a propriedade subject como virtual e ficava puxando ela também

            return levels.Adapt<List<LevelDTO>>();
        }

        // GET: api/Levels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LevelDTO>> GetLevel(int id)
        {
            var level = await _context.Levels.Where(x=>x.Id == id).Include(x => x.SubjectTheme)
                                                    .ThenInclude(x => x.Subject).AsNoTracking().FirstOrDefaultAsync();

            if (level == null)
            {
                return NotFound();
            }

            return level.Adapt<LevelDTO>();
        }

        // PUT: api/Levels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLevel(int id, Level level)
        {
            if (id != level.Id)
            {
                return BadRequest();
            }

            _context.Entry(level).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LevelExists(id))
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

        // POST: api/Levels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Level>> PostLevel(Level level)
        {
            _context.Levels.Add(level);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLevel", new { id = level.Id }, level);
        }

        // DELETE: api/Levels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLevel(int id)
        {
            var level = await _context.Levels.FindAsync(id);
            if (level == null)
            {
                return NotFound();
            }

            _context.Levels.Remove(level);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LevelExists(int id)
        {
            return _context.Levels.Any(e => e.Id == id);
        }
    }
}
