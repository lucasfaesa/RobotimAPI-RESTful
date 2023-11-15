using API_Mestrado_Lucas.Context;
using API_Mestrado_Lucas.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TeachersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Teachers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeacherDTO>> GetTeacher(int id)
        {
            var student = await _context.Teachers.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student.Adapt<TeacherDTO>();
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherDTO>>> GetAllTeachers()
        {
            var teacher = await _context.Teachers.ToListAsync();

            return teacher.Adapt<List<TeacherDTO>>();
        }

        [HttpGet("GetTeacherInfos/{id}")]
        public async Task<ActionResult<TeacherCompleteInfoDTO>> GetTeacherInfos(int id)
        {
            var teachers = await _context.Teachers.Where(x => x.Id == id).FirstOrDefaultAsync();

            return teachers.Adapt<TeacherCompleteInfoDTO>();
        }


        [HttpGet("GetAllTeachersInfos")]
        public async Task<ActionResult<IEnumerable<TeacherCompleteInfoDTO>>> GetAllTeachersInfos()
        {
            var teachers = await _context.Teachers.ToListAsync();

            return teachers.Adapt<List<TeacherCompleteInfoDTO>>();
        }

        [HttpGet("GetTeacherAndStudents/{id}")]
        public async Task<ActionResult<TeacherDTO>> GetTeacherAndStudents(int id)
        {
            var student = await _context.Teachers.Where(x => x.Id == id).Include(x => x.Students).ThenInclude(x => x.GroupClass).Include(x=>x.GroupClasses).AsNoTracking().FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound();
            }

            return student.Adapt<TeacherDTO>();
        }

        [HttpGet("GetAllTeacherAndStudents")]
        public async Task<ActionResult<IEnumerable<TeacherDTO>>> GetAllTeachersAndStudents()
        {
            var teachers = await _context.Teachers.Include(x=>x.Students).ThenInclude(x=>x.GroupClass).Include(x => x.GroupClasses).AsNoTracking().ToListAsync();

            return teachers.Adapt<List<TeacherDTO>>();
        }

        // POST: api/Teachers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TeacherDTO>> PostTeacher(TeacherCompleteInfoDTO teacher)
        {
            if (_context.Teachers.Any(x => x.Username == teacher.Username))
            {
                return BadRequest("Username already exists!");
            }

            _context.Teachers.Add(teacher.Adapt<Teacher>());

            teacher.CreationDate = DateTime.Now;

            await _context.SaveChangesAsync();

            var teacherObj = CreatedAtAction("GetTeacherInfos", new { id = teacher.Id }, teacher);

            var teacherDTO = teacherObj.Adapt<TeacherDTO>();

            return teacherDTO;
        }

        [HttpPost("CreateTeacher")]
        public async Task<ActionResult<TeacherDTO>> CreateTeacher(TeacherCompleteInfoDTO teacher)
        {
            if (_context.Teachers.Any(x => x.Username == teacher.Username))
            {
                return BadRequest("Username already exists!");
            }

            var newTeacher = teacher.Adapt<Teacher>();

            newTeacher.Password = ComputeSHA512Hash(teacher.Password);
            newTeacher.CreationDate = DateTime.Now;

            _context.Teachers.Add(newTeacher);
            await _context.SaveChangesAsync();

            var teacherObj = CreatedAtAction("GetTeacherInfos", new { id = teacher.Id }, teacher);

            var teacherDTO = teacherObj.Adapt<TeacherDTO>();

            return teacherDTO;
        }

        public static string ComputeSHA512Hash(string input)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha512.ComputeHash(inputBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                return hash;
            }
        }


        // PUT: api/Teachers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher(int id, Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return BadRequest();
            }

            _context.Entry(teacher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(id))
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


        // DELETE: api/Teachers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher == null)
            {
                return NotFound();
            }

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<TeacherDTO>> TeacherLogin(TeacherLoginDTO teacherLoginDTO)
        {
            var result = await _context.Teachers.Where(x => x.Username.ToLower() == teacherLoginDTO.Username.ToLower()
                                                        && x.Password == ComputeSHA512Hash(teacherLoginDTO.Password)).Include(x=>x.GroupClasses).Include(x=>x.Students).FirstOrDefaultAsync();

            if (result == null)
            {
                return NotFound("User or Password Invalid!");
            }

            var teacherDTO = result.Adapt<TeacherDTO>();

            teacherDTO.LastLoginDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return teacherDTO;
        }

    }
}
