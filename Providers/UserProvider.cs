using project_backend.Models.User;
using project_backend.Repos;
using System.Collections.Generic;
using System.Linq;

namespace project_backend.Providers
{
    public class UserProvider
    {
        private readonly DatabaseContext _dbContext;

        public UserProvider(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public int getUserByCredentials(UserDAO DaoUser)
        {
            var query = from user in _dbContext.Users
                        where user.Username == DaoUser.Username && user.Password == DaoUser.Password
                        select user.Id;
            List<int> result = query.ToList<int>();
            return result.Count != 0 ? result[0] : -1;
        }
    }
}
