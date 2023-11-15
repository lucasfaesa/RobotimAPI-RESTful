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
using System.Text;
using System.Security.Cryptography;

namespace API_Mestrado_Lucas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IStudentService _studentService;

        public StudentsController(AppDbContext context, IStudentService studentService)
        {
            _context = context;
            _studentService = studentService;
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDTO>> GetStudent(int id)
        {
            var student = await _context.Students.Include(x => x.GroupClass).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return student.Adapt<StudentDTO>();
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {
            var student = await _context.Students.Include(x => x.GroupClass).AsNoTracking().ToListAsync();

            return student.Adapt<List<StudentDTO>>();
        }

        [HttpGet("GetStudentCompleteInfo/{id}")]
        public async Task<ActionResult<StudentCompleteInfoDTO>> GetStudentInfos(int id)
        {
            var student = await _context.Students.Include(x => x.GroupClass).Include(x => x.Teacher).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            //limpando algus dados que vieram com o include
            student.Teacher.Students.Clear();
            student.GroupClass.Students.Clear();

            if (student == null)
            {
                return NotFound();
            }

            return student.Adapt<StudentCompleteInfoDTO>();
        }


        [HttpGet("GetAllStudentsInfos")]
        public async Task<ActionResult<IEnumerable<StudentCompleteInfoDTO>>> GetAllStudentsInfos()
        {
            var students = await _context.Students.Include(x => x.GroupClass).Include(x => x.Teacher).AsNoTracking().ToListAsync();

            foreach (var student in students)
            {
                //limpando algus dados que vieram com o include
                student.Teacher.Students.Clear();
                student.GroupClass.Students.Clear();
            }

            return students.Adapt<List<StudentCompleteInfoDTO>>();
        }

        [HttpGet("GetStudentsByTeacherId/{id}")]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetTeacherStudents(int id)
        {
            var student = await _context.Students.Include(x => x.GroupClass).Where(x => x.TeacherId == id).AsNoTracking().ToListAsync();

            if (student == null)
            {
                return NotFound();
            }

            return student.Adapt<List<StudentDTO>>();
        }

        [HttpGet("GetStudentsByGroupClassId/{id}")]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudentsByGroupClass(int id)
        {
            var student = await _context.Students.Where(x => x.GroupClassId == id).AsNoTracking().ToListAsync();

            if (student == null)
            {
                return NotFound();
            }

            return student.Adapt<List<StudentDTO>>();
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudentDTO>> PostStudent(StudentCompleteInfoDTO student)
        {
            if (_context.Students.Any(x => x.Username == student.Username && x.TeacherId == student.TeacherId)
                || !_context.Teachers.Any(x => x.Id == student.TeacherId))
            {
                return BadRequest("Username of student already exists for this teacher or Teacher doesn't exist");
            }
            student.CreationDate = DateTime.Now;
            student.LastLoginDate = DateTime.Now;

            var newStudent = student.Adapt<Student>();

            newStudent.Password = ComputeSHA512Hash(student.Password);
            newStudent.CreationDate = DateTime.Now;

            _context.Students.Add(newStudent);

            await _context.SaveChangesAsync();

            var studentObj = CreatedAtAction("GetStudentInfos", new { id = student.Id }, student);

            var studentDto = studentObj.Adapt<StudentDTO>();

            return studentDto;
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

        [HttpPost("BatchPostStudents/")]
        public async Task<ActionResult<ICollection<StudentDTO>>> BatchPostStudent(List<StudentCompleteInfoDTO> students)
        {
            List<Student> studentsToBeAdded = students.Adapt<List<Student>>();

            for (int i = 0; i < studentsToBeAdded.Count; i++)
            {
                if (_context.Students.Any(x => x.Username == studentsToBeAdded[i].Username && x.TeacherId == studentsToBeAdded[i].TeacherId)
                || !_context.Teachers.Any(x => x.Id == studentsToBeAdded[i].TeacherId))
                {
                    studentsToBeAdded.Remove(studentsToBeAdded[i]);
                }
            }

            if (studentsToBeAdded.Count == 0)
            {
                return NotFound();
            }

            foreach (var student in studentsToBeAdded)
            {
                student.Name = student.Name.Trim();
                student.Password = ComputeSHA512Hash(student.Password);
                student.CreationDate = DateTime.Now;
                student.LastLoginDate = DateTime.Now;
            }

            _context.Students.AddRange(studentsToBeAdded);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        [HttpPost("EditStudent/")]
        public async Task<ActionResult<StudentDTO>> EditStudent(StudentEditInfoDTO student)
        {
            var std = await _context.Students.FirstOrDefaultAsync(x => x.Id == student.Id);

            if (student == null)
            {
                return NotFound();
            }

            if (_context.Students.Any(x => x.Username == student.Username && x.Id != student.Id)) { //se ja existir um estudante com o mesmo login
                return BadRequest();
            }

            std.GroupClassId = student.GroupClassId;
            std.Name = student.Name.Trim();
            std.Username = student.Username;
            std.Password = ComputeSHA512Hash(student.Password);

            await _context.SaveChangesAsync();

            return Ok(std.Adapt<StudentDTO>());
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("BatchDeleteStudents/")]
        public async Task<ActionResult<ICollection<StudentDTO>>> BatchDelete(List<StudentDTO> students)
        {
            List<Student> studentsToBeRemoved = new();

            foreach (var student in students)
            {
                studentsToBeRemoved.Add(await _context.Students.FindAsync(student.Id));
            }

            if (studentsToBeRemoved.Count == 0)
            {
                return NotFound();
            }

            _context.Students.RemoveRange(studentsToBeRemoved);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }

        [HttpPost("StudentLogin")]
        public async Task<ActionResult<StudentDTO>> StudentLogin(StudentLoginDTO studentLoginDto)
        {
            var result = await _studentService.StudentLogin(studentLoginDto);

            if (result == null)
            {
                return NotFound("User or Password Invalid!");
            }

            return result;
        }

        [HttpGet("GetCompletedLevels/{id}")]
        public async Task<ICollection<LevelDTO>> GetCompletedLevels(int id)
        {
            var result = await _context.Sessions.Where(x => x.StudentId == id && x.Finished.Value && x.LevelId != null).Include(x=>x.Level).AsNoTracking().ToListAsync();

            if (result == null)
            {
                return new List<LevelDTO>();
            }

            result = result.GroupBy(x => x.LevelId).Select(g => g.First()).ToList();//ao inves de usar distinct, usei group by + select

            List<Level> completedLevels = new();

            foreach(var session in result)
            {
                completedLevels.Add(session.Level);
            }

            return completedLevels.Adapt<List<LevelDTO>>();

        }

        [HttpGet("GetCompletedQuizes/{id}")]
        public async Task<ICollection<QuizDTO>> GetCompletedQuizes(int id)
        {
            var result = await _context.Sessions.Where(x => x.StudentId == id && x.Finished.Value && x.QuizId != null).Include(x => x.Quiz).AsNoTracking().ToListAsync();

            if (result == null)
            {
                return new List<QuizDTO>();
            }

            result = result.GroupBy(x => x.QuizId).Select(g => g.First()).ToList();//ao inves de usar distinct, usei group by + select

            List<Quiz> completedQuizes = new();

            foreach (var session in result)
            {
                completedQuizes.Add(session.Quiz);
            }

            return completedQuizes.Adapt<List<QuizDTO>>();

        }

        [HttpGet("GetTotalScore/{id}")]
        public async Task<int> GetTotalScore(int id)
        {
            var result = await _context.Sessions.Where(x => x.StudentId == id && x.Finished.Value && x.Score > 0).AsNoTracking().ToListAsync();

            if (result == null)
                return 0;

            var totalScore = result.Sum(x => x.Score);

            return totalScore;
        }


        [HttpPost("TemporaryLogin")]
        public async Task<ActionResult<StudentDTO>> TemporaryLogin(StudentLoginDTO studentLoginDto)
        {
            Console.WriteLine("Tentativa de Login Temporário");
            var result = await _studentService.TemporaryLogin(studentLoginDto);

            if (result == null)
            {
                var newStudent = new Student
                {
                    Name = studentLoginDto.Username.Trim(),
                    Password = studentLoginDto.Password,
                    Username = studentLoginDto.Username,
                    GroupClassId = _context.GroupClasses.Where(x=>x.TeacherId == 1).OrderBy(x=>x.Id).LastOrDefault().Id,
                    TeacherId = 1,
                    CreationDate = DateTime.Now,
                    LastLoginDate = DateTime.Now,
                };

                _context.Students.Add(newStudent);

                result = newStudent;
            }
            else
            {
                result.LastLoginDate = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            Console.WriteLine("Login Temporário Sucesso");
            return result.Adapt<StudentDTO>();
        }

    }
}
