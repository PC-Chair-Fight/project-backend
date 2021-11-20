using project_backend.Models.JobImage;
using System;
using System.Collections.Generic;

namespace project_backend.Models.JobController.GetJobDetails
{
    public class GetJobDetailsResponseObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime PostDate { get; set; }
        public bool Done { get; set; } = false;

        public ICollection<JobImageDAO> Images { get; set; }

    }
}
