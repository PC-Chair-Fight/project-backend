using project_backend.Models.DAOs;

namespace project_backend.Providers.WorkerApplicationProvider
{
    public interface IWorkerApplicationProvider
    {
        public WorkerApplicationDAO AddWorkerApplication(int userId);

        public void AcceptWorkerApplication(int userId);

        public void CancelWorkerApplication(int userId);

        public void DeleteWorkerApplication(int userId);

    }
}
