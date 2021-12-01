using project_backend.Models.Bid;
using System.Collections.Generic;
using System.Linq;

namespace project_backend.Models.JobController.GetJobBids
{
    public class GetJobBidsResponseObject
    {
        public List<JobBidDTO> Bids { get; set; }
        public GetJobBidsResponseObject(IQueryable<BidDAO> bidsInput)
        {
            Bids = bidsInput.Select(b => new JobBidDTO
            {
                Id = b.Id,
                JobId = b.JobId,
                Sum = b.Sum,
                WorkerId = b.WorkerId
            }
            ).ToList();
        }
    }

    public class JobBidDTO
    {
        public int Id { get; set; }
        public float Sum { get; set; }
        public int WorkerId { get; set; }
        public int JobId { get; set; }
    }
}
