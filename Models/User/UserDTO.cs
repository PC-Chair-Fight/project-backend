using System;

namespace project_backend.Models.User
{
    public class UserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
