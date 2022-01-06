using Microsoft.AspNetCore.Hosting;
using project_backend.Models.Exceptions;
using project_backend.Models.Job;
using project_backend.Models.JobImage;
using project_backend.Repos;
using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace project_backend.Providers.JobProvider
{
    public class JobProvider : IJobProvider
    {
        private readonly DatabaseContext _dbContext;
        private readonly IWebHostEnvironment _environment;

        public JobProvider(DatabaseContext databaseContext, IWebHostEnvironment environment)
        {
            _dbContext = databaseContext;
            _environment = environment;
        }

        public IQueryable<JobDAO> QueryJobs() =>
            _dbContext.Jobs.AsQueryable();

        public JobDAO AddJob(string name, string description, int userId, byte[][] images)
        {
            JobDAO newJob = new JobDAO
            {
                Name = name,
                Description = description,
                UserId = userId
            };
            
            AddImages(newJob, images);

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
            catch (InvalidOperationException)
            {
                throw new ResourceNotFoundException("Job with that id doesn't exist!");
            }

        }
        private JobImageDAO[] AddImages(JobDAO job, byte[][] images)
        {
            const string RelativeJobImagesFolder = "Images/Jobs";

            if (images == null)
                return new JobImageDAO[0];
            
            job.Images = new List<JobImageDAO>();

            var jobImagesFolder = Path.Combine(_environment.WebRootPath, RelativeJobImagesFolder);

            foreach (var image in images)
            {
                var filename = Guid.NewGuid().ToString() + ".jpg"; // random filename
                var localPath = Path.Combine(jobImagesFolder, filename);

                while (File.Exists(localPath)) // this should almost never happen - https://en.wikipedia.org/wiki/Universally_unique_identifier
                {
                    filename = Guid.NewGuid().ToString() + ".jpg";
                    localPath = Path.Combine(jobImagesFolder, filename);
                }
                File.WriteAllBytes(localPath, image);

                var jobImage = new JobImageDAO
                {
                    Job = job,
                    URL = $"{RelativeJobImagesFolder}/{filename}",
                };

                job.Images.Add(jobImage);
            }

            return job.Images.ToArray();
        }
    }
}
