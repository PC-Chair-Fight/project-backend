using project_backend.Models.User;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_backend.Models.Job
{
    [Table("jobs")]
    public class JobDAO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime PostDate { get; set; }
        public bool Done { get; set; }

        [Required]
        public int UserId { get; set; }
        public UserDAO User { get; set; }
    }
}
