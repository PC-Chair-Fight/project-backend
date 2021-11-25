namespace project_backend.Models.Exceptions
{
    public class ResourceNotFoundException : BaseException
    {
        public ResourceNotFoundException(string message = "Resource not found") : base(message) { }
    }
}
