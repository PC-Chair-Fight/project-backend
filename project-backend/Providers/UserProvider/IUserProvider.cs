using project_backend.Models.User;
using System;

namespace project_backend.Providers.UserProvider
{
    public interface IUserProvider
    {
        public int getUserIdByCredentials(string email, string password);
        public int createUser(string firstName, string lastName, DateTime dateOfBirth, string email, string password);

        public UserDAO GetUserById(int userId);
    }
}
