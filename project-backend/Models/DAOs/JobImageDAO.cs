using project_backend.Models.Job;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_backend.Models.JobImage
{
    [Table("job_images")]
    public class JobImageDAO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string URL { get; set; }

        public JobDAO Job { get; set; }
    }
}
