using project_backend.Models.Job;
using System.Linq;

namespace project_backend.Providers.JobProvider
{
    public interface IJobProvider
    {
        public IQueryable<JobDAO> QueryJobs();
    }
}
