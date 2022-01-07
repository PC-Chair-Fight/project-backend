using project_backend.Models.Worker;
using project_backend.Repos;
using System;
using System.Linq;

namespace project_backend.Providers.WorkerProvider
{
    public class WorkerProvider : IWorkerProvider
    {
        private readonly DatabaseContext _dbContext;

        public WorkerProvider(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public WorkerDAO AddWorker(int userId)
        {
            var existingWorker = _dbContext.Workers.FirstOrDefault(w => w.UserId == userId);

            //If the user isn't yet a worker we add it.
            if (existingWorker == null)
            {
                var newWorker = new WorkerDAO { UserId = userId };
                _dbContext.Workers.Add(newWorker);
                _dbContext.SaveChanges();
                return newWorker;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public bool UserIsWorker(int userId)
        {
            return _dbContext.Workers.Any(w => w.UserId == userId);
        }
    }
}
