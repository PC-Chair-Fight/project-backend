using project_backend.Models.Bid;
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
                Worker = new WorkerDTO
                {
                    User = new UserDTO
                    {
                        FirstName = b.Worker.User.FirstName,
                        LastName = b.Worker.User.LastName,
                        ProfileImage = b.Worker.User.ProfileImage
                    }
                }
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

    public class UserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImage { get; set; }
    }

    public class WorkerDTO
    {
        public UserDTO User { get; set; }
    }
}
