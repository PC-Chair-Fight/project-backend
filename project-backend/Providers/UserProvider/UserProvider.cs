using project_backend.Models.Exceptions;
using project_backend.Models.User;
using project_backend.Repos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace project_backend.Providers.UserProvider
{
    public class UserProvider : IUserProvider
    {
        private readonly DatabaseContext _dbContext;

        public UserProvider(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public int createUser(string firstName, string lastName, DateTime dateOfBirth, string email, string password)
        {
            var findUserByEmailQuery = from user in _dbContext.Users
                                       where user.Email == email
                                       select user.Id;

            List<int> result = findUserByEmailQuery.ToList();
            if (result.Count != 0)
            {
                return -1;
            }

            UserDAO userDAO = new UserDAO
            {
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Email = email,
                Password = password
            };
            _dbContext.Users.Add(userDAO);
            _dbContext.SaveChanges();
            return userDAO.Id;
        }

        public UserDAO GetUserById(int userId)
        {
            var user = _dbContext.Users.Find(userId);
            if (user == null)
            {
                throw new ResourceNotFoundException("User could not be found");
            }
            return user;
        }

        public int getUserIdByCredentials(string email, string password)
        {
            var query = from user in _dbContext.Users
                        where user.Email == email && user.Password == password
                        select user.Id;
            List<int> result = query.ToList();
            return result.Count != 0 ? result[0] : -1;
        }
    }
}
