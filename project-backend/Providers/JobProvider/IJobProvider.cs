using project_backend.Models.Bid;
using project_backend.Models.Job;
using System.Linq;

namespace project_backend.Providers.JobProvider
{
    public interface IJobProvider
    {
        public IQueryable<JobDAO> QueryJobs();
        public JobDAO EditJob(int id, string newName, string newDescription, int authorId);
        public JobDAO AddJob(string name, string description, int authorId, byte[][] images);
        public JobDAO GetJobById(int jobId);
    }
}
