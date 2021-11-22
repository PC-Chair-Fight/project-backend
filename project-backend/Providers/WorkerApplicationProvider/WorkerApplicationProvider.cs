using project_backend.Models.DAOs;
using project_backend.Models.Exceptions;
using project_backend.Models.Worker;
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
            var userIsAlreadyWorker = _dbContext.Workers.Find(userId);

            //Even if the endpoint is guarded by a user role, some users which didn't get their updated token could still call this.
            if (userIsAlreadyWorker != null)
            {
                throw new UserIsAlreadyWorker();
            }

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

        public void AcceptWorkerApplication(int userId)
        {
            var workerApplication = _dbContext.WorkerApplications.Find(userId);
            var userIsAlreadyWorker = _dbContext.Workers.Any(a => a.UserId == userId);

            if (userIsAlreadyWorker)
            {
                throw new UserIsAlreadyWorker();
            }

            if (workerApplication != null)
            {
                var transaction = _dbContext.Database.BeginTransaction();

                _dbContext.WorkerApplications.Remove(workerApplication);
                _dbContext.SaveChanges();

                var newWorker = new WorkerDAO { UserId = userId };
                _dbContext.Workers.Add(newWorker);
                _dbContext.SaveChanges();

                transaction.Commit();
            }
            else
            {
                throw new ResourceNotFoundException("Worker application isn't available");
            }

        }


        public void DeleteWorkerApplication(int userId)
        {
            var workerApplication = _dbContext.WorkerApplications.Find(userId);

            if (workerApplication != null)
            {
                _dbContext.WorkerApplications.Remove(workerApplication);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new ResourceNotFoundException("Worker application isn't available");
            }
        }

        public void CancelWorkerApplication(int userId)
        {
            var workerApplication = _dbContext.WorkerApplications.Find(userId);
            var userIsAlreadyWorker = _dbContext.Workers.Find(userId);

            //Even if the endpoint is guarded by a user role, some users which didn't get their updated token could still call this.
            if (userIsAlreadyWorker != null)
            {
                throw new UserIsAlreadyWorker();
            }

            if (workerApplication != null)
            {
                _dbContext.WorkerApplications.Remove(workerApplication);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new ResourceNotFoundException("Worker application isn't available");
            }
        }
    }
}
