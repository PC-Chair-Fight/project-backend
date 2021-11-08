﻿using System.ComponentModel.DataAnnotations;

namespace project_backend.Models.UserController.Login
{
    public class LoginRequestQueryObject
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
