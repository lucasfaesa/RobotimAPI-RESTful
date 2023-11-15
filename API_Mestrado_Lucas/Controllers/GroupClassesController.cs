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
    public class GroupClassesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GroupClassesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/GroupClasses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupClassDTO>> GetGroupClass(int id)
        {
            var groupClass = await _context.GroupClasses.FindAsync(id);

            if (groupClass == null)
            {
                return NotFound();
            }


            return groupClass.Adapt<GroupClassDTO>();
        }

        // GET: api/GroupClasses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupClassDTO>>> GetGroupClasses()
        {
            var groupClasses = await _context.GroupClasses.Include(x=>x.GroupClassSubjectThemes).ThenInclude(x => x.SubjectTheme).ThenInclude(x=>x.Levels).ToListAsync();

            return groupClasses.Adapt<List<GroupClassDTO>>();
        }

        // GET: api/GroupClasses/5
        [HttpGet("GetGroupClassAndLevelsById/{id}")]
        public async Task<ActionResult<GroupClassDTO>> GetGroupClassAndLevelsById(int id)
        {
            var groupClass = await _context.GroupClasses.Where(x=>x.Id == id).Include(x => x.GroupClassSubjectThemes)
                                                                                    .ThenInclude(x=>x.SubjectTheme)
                                                                                        .ThenInclude(x=>x.Levels)
                                                                                                .Include(x => x.GroupClassSubjectThemes)
                                                                                                    .ThenInclude(x => x.SubjectTheme)
                                                                                                        .ThenInclude(x=>x.Subject).AsNoTracking().FirstOrDefaultAsync();

            if (groupClass == null)
            {
                return NotFound();
            }

            return groupClass.Adapt<GroupClassDTO>();
        }

        // GET: api/GroupClasses
        [HttpGet("GetGroupClassesByTeacherId/{id}")]
        public async Task<ActionResult<IEnumerable<GroupClassDTO>>> GetGroupClassesByTeacherId(int id)
        {
            var groupClasses = await _context.GroupClasses.Where(x => x.TeacherId == id).Include(x=>x.Students).AsNoTracking().ToListAsync();

            return groupClasses.Adapt<List<GroupClassDTO>>();
        }

        // PUT: api/GroupClasses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupClass(int id, GroupClass groupClass)
        {
            if (id != groupClass.Id)
            {
                return BadRequest();
            }

            _context.Entry(groupClass).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupClassExists(id))
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

        // POST: api/GroupClasses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GroupClassDTO>> PostGroupClass(GroupClassDTO groupClass)
        {
            _context.GroupClasses.Add(groupClass.Adapt<GroupClass>());
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroupClass", new { id = groupClass.Id }, groupClass);
        }

        [HttpPost("BatchPostGroupClasses/")]
        public async Task<ActionResult<ICollection<GroupClassDTO>>> BatchPostGroupClass(List<GroupClassDTO> groupClasses)
        {
            List<GroupClass> groupClassesToBeAdded = groupClasses.Adapt<List<GroupClass>>();

            groupClassesToBeAdded.ForEach(x => x.Name = x.Name.Trim());

            if (groupClassesToBeAdded.Count == 0)
            {
                return NotFound();
            }

            _context.GroupClasses.AddRange(groupClassesToBeAdded);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/GroupClasses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroupClass(int id)
        {
            var groupClass = await _context.GroupClasses.FindAsync(id);
            if (groupClass == null)
            {
                return NotFound();
            }

            _context.GroupClasses.Remove(groupClass);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("BatchDeleteGroupClasses/")]
        public async Task<ActionResult<ICollection<GroupClassDTO>>> BatchDelete(List<GroupClassDTO> groupClasses)
        {
            List<GroupClass> groupClassesToBeRemoved = new();
            List<StudentDTO> studentsToRemoveGroupClass = new();

            foreach (var groupClass in groupClasses)
            {
                groupClassesToBeRemoved.Add(await _context.GroupClasses.Where(x=>x.Id == groupClass.Id).Include(x=>x.Students).AsNoTracking().FirstOrDefaultAsync());
            }

            if (groupClassesToBeRemoved.Count == 0)
            {
                return NotFound();
            }

            foreach (var groupClass in groupClassesToBeRemoved)
            {
                studentsToRemoveGroupClass.AddRange(groupClass.Students.Adapt<List<StudentDTO>>());
            }

            foreach (var student in studentsToRemoveGroupClass) //removendo os groupclasses do alunos ja criados
            {
                student.GroupClass = null;
            }


            _context.GroupClasses.RemoveRange(groupClassesToBeRemoved);
            await _context.SaveChangesAsync();

            return Ok();
        }


        private bool GroupClassExists(int id)
        {
            return _context.GroupClasses.Any(e => e.Id == id);
        }
    }
}
