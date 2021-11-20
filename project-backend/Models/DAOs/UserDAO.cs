using Microsoft.EntityFrameworkCore;
using project_backend.Models.Job;
using project_backend.Models.Worker;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_backend.Models.User
{
    [Table("users")]
    [Index(nameof(Email), IsUnique = true)]
    public class UserDAO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<JobDAO> Jobs { get; set; }

        public WorkerDAO Worker { get; set; }

    }
}
