using Microsoft.EntityFrameworkCore;
using project_backend.Models.Job;
using project_backend.Models.User;

namespace project_backend.Repos
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        //public DbSet<WeatherForecast> Forecasts { get; set; }
        public DbSet<UserDAO> Users { get; set; }
        public DbSet<JobDAO> Jobs { get; set; }
    }
}
