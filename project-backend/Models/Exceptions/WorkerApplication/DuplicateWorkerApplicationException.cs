namespace project_backend.Models.Exceptions
{
    public class DuplicateWorkerApplicationException : BaseException
    {
        public DuplicateWorkerApplicationException(string message = "User already applied for a worker account") : base(message) { }
    }
}
