using System.ComponentModel.DataAnnotations;

namespace project_backend.Models.JobsController.AddJob
{
    public class AddJobQueryObject
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
