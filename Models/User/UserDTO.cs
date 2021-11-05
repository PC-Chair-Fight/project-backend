using System;

namespace project_backend.Models.User
{
    public class UserDTO
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
