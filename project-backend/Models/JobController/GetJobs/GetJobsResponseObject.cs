using System;
using System.ComponentModel.DataAnnotations;

namespace project_backend.Models.JobController.GetJobs
{
    public class GetJobsResponseObject
    {
        public class FetchedUser
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string ProfileImage { get; set; }
        }

        public class FetchedWorker
        {
            public FetchedUser User { get; set; }
        }

        public class FetchedBid
        {
            public FetchedWorker Worker { get; set; }
            public float Sum { get; set; }
        }

        public class GetJobsResponseEntry
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime PostDate { get; set; }
            public bool Done { get; set; }
            public FetchedUser User { get; set; }
            public FetchedBid[] Bids { get; set; }
        }

        [Required]
        public GetJobsResponseEntry[] jobs { get; set; }
    }
}
