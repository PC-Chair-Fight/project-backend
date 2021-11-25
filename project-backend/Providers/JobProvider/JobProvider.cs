using project_backend.Models.Exceptions;
using project_backend.Models.Job;
using project_backend.Repos;
using System;
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

        public JobDAO AddJob(string name, string description, int userId)
        {
            JobDAO newJob = new JobDAO
            {
                Name = name,
                Description = description,
                UserId = userId
            };

            _dbContext.Jobs.Add(newJob);
            _dbContext.SaveChanges();

            return newJob;
        }

        public JobDAO GetJobById(int jobId)
        {
            JobDAO requiredJob;
            try
            {
                requiredJob = _dbContext.Jobs.AsQueryable().Where(job => job.Id == jobId).First();
            }
            catch (InvalidOperationException)
            {
                throw new ResourceNotFoundException("Job with that id doesn't exist!");
            }
            return requiredJob;
        }
    }
}
