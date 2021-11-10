using project_backend.Models.Job;
using project_backend.Repos;
using System.Linq;

namespace project_backend.Providers.JobProvider
{
    public class JobProvider : IJobProvider
    {
        private readonly DatabaseContext _dbContext;

        public JobProvider(DatabaseContext databaseContext) =>
            _dbContext = databaseContext;

        public IQueryable<JobDAO> QueryJobs() =>
            _dbContext.Jobs.AsQueryable();
    }
}
