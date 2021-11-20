using project_backend.Models.User;
<<<<<<< HEAD
=======
using System;
>>>>>>> develop
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_backend.Models.Worker
{
<<<<<<< HEAD
    [Table("users")]
    public class WorkerDAO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public UserDAO User { get; set; }
=======
    [Table("workers")]
    public class WorkerDAO
    {
        [Key]
        public int UserId { get; set; }
        public UserDAO User { get; set; }
        public DateTime WorkerSince { get; set; } = DateTime.Now;

>>>>>>> develop
    }
}
