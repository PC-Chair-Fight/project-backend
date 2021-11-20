using project_backend.Models.Worker;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_backend.Models.Bid
{
    [Table("jobs")]
    public class BidDAO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public float Sum { get; set; }

        public WorkerDAO Worker { get; set; }
    }
}
