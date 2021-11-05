using project_backend.Models.User;
using project_backend.Repos;

namespace project_backend.Providers
{
    public class UserProvider
    {
        private readonly DatabaseContext _dbContext;

        public UserProvider(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public int getUserByCredentials(UserDAO User)
        {
            return -1;
        }
    }
}
