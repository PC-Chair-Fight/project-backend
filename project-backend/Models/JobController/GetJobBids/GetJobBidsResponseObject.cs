using project_backend.Models.Bid;
using project_backend.Models.User;
using System.Collections.Generic;
using System.Linq;

namespace project_backend.Models.JobController.GetJobBids
{
    public class GetJobBidsResponseObject
    {
        public List<JobBidDTO> Bids { get; set; }
        public GetJobBidsResponseObject(IList<BidDAO> bidsInput)
        {
            Bids = bidsInput.Select(b => new JobBidDTO
            {
                Id = b.Id,
                JobId = b.JobId,
                Sum = b.Sum,
                Worker = new WorkerDTO(b.Worker.User)
            }
            ).ToList();
        }
    }

    public class JobBidDTO
    {
        public int Id { get; set; }
        public float Sum { get; set; }
        public WorkerDTO Worker { get; set; }
        public int JobId { get; set; }
    }

    public class WorkerDTO
    {
        public WorkerDTO(UserDAO user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

}
