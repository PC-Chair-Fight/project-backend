using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_backend.Models.User
{
    public class UserDAO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string Password { get; set; }
    }
}
