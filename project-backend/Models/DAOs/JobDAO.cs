using project_backend.Models.Bid;
using project_backend.Models.JobImage;
using project_backend.Models.User;
using System;
using System.Collections.Generic;
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
        public DateTime PostDate { get; set; } = DateTime.Now;
        public bool Done { get; set; } = false;

        [Required]
        public int UserId { get; set; }
        public UserDAO User { get; set; }

        public ICollection<JobImageDAO> Images { get; set; }

        public ICollection<BidDAO> Bids { get; set; }
    }
}
