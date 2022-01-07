namespace project_backend.Models.Exceptions
{
    public class UserIsAlreadyWorker : BaseException
    {
        public UserIsAlreadyWorker(string message = "The user is already a worker.") : base(message) { }

    }
}
