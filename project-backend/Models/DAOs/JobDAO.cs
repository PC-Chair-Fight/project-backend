using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_backend.Models.User
{
    [Table("jobs")]
    public class JobDAO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateOfPost { get; set; }
        [DefaultValue(false)]
        public bool Done { get; set; }
        public int AuthorId { get; set; }
        public UserDAO Author { get; set; }
    }
}
