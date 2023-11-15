using API_Mestrado_Lucas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas.Services.Interfaces
{
    public interface IStudentService
    {
        Task<StudentDTO> StudentLogin(StudentLoginDTO studentLoginDto);

        Task<StudentCompleteInfoDTO> GetStudentWithSessionsById(int id);

        Task<Student> TemporaryLogin(StudentLoginDTO studentLoginDto);
    }

}
