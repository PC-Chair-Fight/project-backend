namespace project_backend.Models.Exceptions.Universal
{
    public class UnauthorizedException : BaseException
    {
        public UnauthorizedException(string message = "User not auhorized") : base(message) { }
    }
}
