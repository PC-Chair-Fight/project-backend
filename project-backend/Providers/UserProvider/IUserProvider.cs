namespace project_backend.Providers.UserProvider
{
    public interface IUserProvider
    {
        public int getUserIdByCredentials(string email, string password);
    }
}
