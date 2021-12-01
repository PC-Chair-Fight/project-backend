using project_backend.Models.Bid;
using project_backend.Models.Exceptions;
using project_backend.Models.Job;
using project_backend.Repos;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

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
            try
            {
                JobDAO requiredJob = _dbContext.Jobs.Include(job => job.Images).Where(job => job.Id == jobId).First();
                return requiredJob;
            }
            catch (InvalidOperationException ex)
            {
                throw new ResourceNotFoundException("Job with that id doesn't exist!");
            }

        }


        public IQueryable<BidDAO> GetJobBids(int jobId)
        {
            var bids = _dbContext.Bids.Where(b => b.JobId == jobId);
            return bids;
        }

    }
}
