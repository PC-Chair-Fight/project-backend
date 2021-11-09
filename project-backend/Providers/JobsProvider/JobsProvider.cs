using Microsoft.AspNetCore.Mvc;
using project_backend.Models.User;
using project_backend.Providers.UserProvider;
using project_backend.Repos;
using System.Linq;
namespace project_backend.Providers.JobsProvider
{
    public class JobsProvider : IJobsProvider
    {
        private readonly DatabaseContext _dbContext;

        public JobsProvider(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public JobDAO AddJob(string name, string description, int authorId)
        {
            JobDAO newJob = new JobDAO
            {
                Name = name,
                Description = description,
                AuthorId = authorId,
                DateOfPost = System.DateTime.Now
            };

            _dbContext.Jobs.Add(newJob);
            _dbContext.SaveChanges();

            return newJob;
        }
    }
}
