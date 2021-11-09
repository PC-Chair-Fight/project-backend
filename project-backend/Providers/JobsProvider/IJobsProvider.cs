using project_backend.Models.User;
using System;

namespace project_backend.Providers.UserProvider
{
    public interface IJobsProvider
    {
        public JobDAO AddJob(string name, string description, int authorId);
    }
}
