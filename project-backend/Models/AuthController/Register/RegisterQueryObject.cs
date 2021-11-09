using System;
using System.ComponentModel.DataAnnotations;

namespace project_backend.Models.AuthController.Register
{
    public class RegisterQueryObject
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
