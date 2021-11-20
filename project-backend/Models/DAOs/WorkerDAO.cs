using project_backend.Models.User;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_backend.Models.Worker
{
    [Table("workers")]
    public class WorkerDAO
    {
        [Key]
        public int UserId { get; set; }
        public UserDAO User { get; set; }
        public DateTime WorkerSince { get; set; } = DateTime.Now;

    }
}
