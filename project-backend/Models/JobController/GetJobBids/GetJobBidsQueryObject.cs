using System.ComponentModel.DataAnnotations;

namespace project_backend.Models.JobController.GetJobBids
{
    public class GetJobBidsQueryObject
    {
        public int? JobId { get; set; }

        public int? Index { get; set; }

        public int? Count { get; set; }
    }
}
