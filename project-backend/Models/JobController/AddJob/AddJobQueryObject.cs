using Microsoft.AspNetCore.Http;

namespace project_backend.Models.JobController.AddJob
{
    public class AddJobQueryObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFileCollection Images { get; set; }
    }
}
