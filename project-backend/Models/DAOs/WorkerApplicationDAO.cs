﻿using project_backend.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_backend.Models.DAOs
{
    [Table("worker_applications")]
    public class WorkerApplicationDAO
    {
        [Key]
        [ForeignKey("users")]
        [Required]
        public int UserId { get; set; }
        public UserDAO User { get; set; }
    }
}
