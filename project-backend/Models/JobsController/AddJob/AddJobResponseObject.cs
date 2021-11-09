using System;

namespace project_backend.Models.JobsController.AddJob
{
    public class AddJobResponseObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateOfPost { get; set; }
        public bool Done { get; set; }
        public int AuthorId { get; set; }
    }
}
