using System;
using System.Collections.Generic;

namespace project_backend.Models.JobController
{
    public class JobResponseObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime PostDate { get; set; }
        public bool Done { get; set; }

        public int UserId { get; set; }

        public ICollection<string> Images { get; set; }
    }
}
