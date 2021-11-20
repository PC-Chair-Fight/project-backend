using project_backend.Models.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_backend.Models.Worker
{
    [Table("users")]
    public class WorkerDAO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public UserDAO User { get; set; }
    }
}
