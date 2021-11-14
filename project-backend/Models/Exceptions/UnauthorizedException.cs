namespace project_backend.Models.Exceptions
{
    public class UnauthorizedException : BaseException
    {
        public UnauthorizedException(string message = "Unauthorized") : base(message) { }
    }
}
