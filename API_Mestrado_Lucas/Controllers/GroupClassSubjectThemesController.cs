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
    public class GroupClassSubjectThemesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GroupClassSubjectThemesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/GroupClassSubjectThemes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupClassSubjectTheme>>> GetGroupClassSubjectThemes()
        {
            return await _context.GroupClassSubjectThemes.ToListAsync();
        }

        // GET: api/GroupClassSubjectThemes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupClassSubjectTheme>> GetGroupClassSubjectTheme(int id)
        {
            var groupClassLevel = await _context.GroupClassSubjectThemes.FindAsync(id);

            if (groupClassLevel == null)
            {
                return NotFound();
            }

            return groupClassLevel;
        }

        // PUT: api/GroupClassSubjectThemes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupClassSubjectTheme(int id, GroupClassSubjectTheme groupClassSubjectTheme)
        {
            if (id != groupClassSubjectTheme.GroupClassId)
            {
                return BadRequest();
            }

            _context.Entry(groupClassSubjectTheme).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupClassSubjectThemeExists(id))
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

        // POST: api/GroupClassSubjectThemes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GroupClassSubjectThemeDTO>> PostGroupClassSubjectTheme(GroupClassSubjectThemeMakerDTO groupClassAndSubjectThemes)
        {
            List<GroupClassSubjectThemeDTO> groupClassLevelList = new();

            for (int i=0; i < groupClassAndSubjectThemes.SubjectThemesIds.Count; i++)
            {
                groupClassLevelList.Add(new GroupClassSubjectThemeDTO
                {
                    GroupClassId = groupClassAndSubjectThemes.GroupClassId,
                    SubjectThemeId = groupClassAndSubjectThemes.SubjectThemesIds[i]
                });
            }

            List<GroupClassSubjectTheme> previousGroupClass = _context.GroupClassSubjectThemes.Where(x => x.GroupClassId == groupClassAndSubjectThemes.GroupClassId).ToList();

            if (previousGroupClass.Count > 0)
                _context.GroupClassSubjectThemes.RemoveRange(previousGroupClass); //removendo as referencias antigas, se existir

            if(groupClassLevelList.Count > 0)
                _context.GroupClassSubjectThemes.AddRange(groupClassLevelList.Adapt<List<GroupClassSubjectTheme>>());

            await _context.SaveChangesAsync();

            return Ok();

        }

        // DELETE: api/GroupClassSubjectThemes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroupClassSubjectTheme(int id)
        {
            var groupClassLevel = await _context.GroupClassSubjectThemes.FindAsync(id);
            if (groupClassLevel == null)
            {
                return NotFound();
            }

            _context.GroupClassSubjectThemes.Remove(groupClassLevel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GroupClassSubjectThemeExists(int id)
        {
            return _context.GroupClassSubjectThemes.Any(e => e.GroupClassId == id);
        }
    }
}
