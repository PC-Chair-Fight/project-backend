using project_backend.Models.DAOs;
using project_backend.Models.Exceptions;
using project_backend.Repos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace project_backend.Providers.WorkerApplicationProvider
{
    public class WorkerApplicationProvider : IWorkerApplicationProvider
    {
        private readonly DatabaseContext _dbContext;

        public WorkerApplicationProvider(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public WorkerApplicationDAO AddWorkerApplication(int userId)
        {
            var userAlreadyApplied = _dbContext.WorkerApplications.Find(userId);
            if (userAlreadyApplied == null)
            {
                var newWorkerApplication = new WorkerApplicationDAO { UserId = userId };
                _dbContext.WorkerApplications.Add(newWorkerApplication);
                _dbContext.SaveChanges();
                return newWorkerApplication;
            }
            else
            {
                throw new DuplicateWorkerApplicationException();
            }
        }
    }
}
