namespace project_backend.Models.Exceptions
{
    //Should be thrown when a user that isn't a worker attempts to do a worker action
    public class NotQualifiedException : BaseException
    {
        public NotQualifiedException(string message = "User not qualified") : base(message) { }
    }
}
