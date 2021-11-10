using System;
using System.ComponentModel.DataAnnotations;

namespace project_backend.Models.JobController.GetJobs
{
    public class GetJobsResponseObject
    {
        public class GetJobsResponseEntry
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime PostDate { get; set; }
            public bool Done { get; set; }
            public int UserId { get; set; }
        }

        [Required]
        public GetJobsResponseEntry[] jobs { get; set; }
    }
}
