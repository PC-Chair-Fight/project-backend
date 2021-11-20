using System.ComponentModel.DataAnnotations;

namespace project_backend.Models.JobController.GetJobDetails
{
    public class GetJobDetailsQueryObject
    {
        [Required]
        public int Id { get; set; }
    }
}
