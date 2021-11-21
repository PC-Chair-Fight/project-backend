using project_backend.Models.Job;
using project_backend.Models.Worker;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_backend.Models.Bid
{
    [Table("bids")]
    public class BidDAO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public float Sum { get; set; }

        [Required]
        public int WorkerId { get; set; }
        public WorkerDAO Worker { get; set; }

        [Required]
        public int JobId { get; set; }
        public JobDAO Job { get; set; }
    }
}
