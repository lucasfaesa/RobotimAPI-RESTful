using System;

namespace API_Mestrado_Lucas
{
    public class StudentLoginDTO : DataTransferObject
    {
        private string username;
        private string password;

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
    }
}
