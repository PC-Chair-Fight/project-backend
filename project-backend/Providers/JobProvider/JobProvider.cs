using project_backend.Models.Bid;
using project_backend.Models.Exceptions;
using project_backend.Models.Job;
using project_backend.Repos;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using project_backend.Models.Exceptions.Universal;

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

        public JobDAO EditJob(int id, string newName, string newDescription, int userId)
        {
            JobDAO correspondingJob = _dbContext.Jobs.Find(id);

            if (correspondingJob == null)
                throw new ResourceNotFoundException("Job not found");

            if (correspondingJob.UserId != userId)
                throw new UnauthorizedException("User isn't the author of the job");

            correspondingJob.Name = newName;
            correspondingJob.Description = newDescription;

            JobDAO updatedJob = _dbContext.Jobs.Update(correspondingJob).Entity;
            _dbContext.SaveChanges();

            return updatedJob;
        }

        public JobDAO GetJobById(int jobId)
        {
            try
            {
                JobDAO requiredJob = _dbContext.Jobs.Include(job => job.Images).Where(job => job.Id == jobId).First();
                return requiredJob;
            }
            catch (InvalidOperationException)
            {
                throw new ResourceNotFoundException("Job with that id doesn't exist!");
            }

        }



    }
}
