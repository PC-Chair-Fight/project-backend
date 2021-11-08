using project_backend.Repos;
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
