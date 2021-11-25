namespace project_backend.Models.Exceptions
{
    public class DuplicateWorkerException : BaseException
    {
        public DuplicateWorkerException(string message = "User is already a worker") : base(message) { }

    }
}
