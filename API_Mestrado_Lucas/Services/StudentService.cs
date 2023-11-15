using API_Mestrado_Lucas.Context;
using API_Mestrado_Lucas.Models;
using API_Mestrado_Lucas.Services.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas.Services
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<StudentDTO> StudentLogin(StudentLoginDTO studentLoginDto)
        {
            var student = await _context.Students.FirstOrDefaultAsync(x => x.Username.ToLower() == studentLoginDto.Username.ToLower() 
                                                                                             && x.Password == ComputeSHA512Hash(studentLoginDto.Password));

            if(student != null)
            {
                student.LastLoginDate = DateTime.Now;
                await _context.SaveChangesAsync();
                var studentDTO = student.Adapt<StudentDTO>();
                return studentDTO;
            }

            return null;            
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

        public async Task<StudentCompleteInfoDTO> GetStudentWithSessionsById(int id)
        {
            var student = await _context.Students.Where(x => x.Id == id).Include(x => x.Sessions).ThenInclude(x => x.Level).AsNoTracking().FirstOrDefaultAsync();

            return student.Adapt<StudentCompleteInfoDTO>();
        }

        public async Task<Student> TemporaryLogin(StudentLoginDTO studentLoginDto)
        {
            var tempStudent = await _context.Students.FirstOrDefaultAsync(x => x.Username.ToLower() == studentLoginDto.Username.ToLower());

            return tempStudent;
        }
    }
}
