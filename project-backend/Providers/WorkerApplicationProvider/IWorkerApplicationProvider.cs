using project_backend.Models.DAOs;

namespace project_backend.Providers.WorkerApplicationProvider
{
    public interface IWorkerApplicationProvider
    {
        public WorkerApplicationDAO AddWorkerApplication(int userId);
    }
}
