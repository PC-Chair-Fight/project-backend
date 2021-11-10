using System;

namespace project_backend.Models.JobController.AddJob
{
    public class AddJobResponseObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime PostDate { get; set; }
        public bool Done { get; set; }
        public int UserId { get; set; }
    }
}
