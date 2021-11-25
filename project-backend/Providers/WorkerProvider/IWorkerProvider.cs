using project_backend.Models.Worker;

namespace project_backend.Providers.WorkerProvider
{
    public interface IWorkerProvider
    {
        public WorkerDAO AddWorker(int userId);

        public bool UserIsWorker(int userId);
    }
}
