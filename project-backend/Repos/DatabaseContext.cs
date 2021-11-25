using Microsoft.EntityFrameworkCore;
using project_backend.Models.Bid;
using project_backend.Models.DAOs;
using project_backend.Models.Job;
using project_backend.Models.User;
using project_backend.Models.Worker;

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
        public DbSet<BidDAO> Bids { get; set; }
        public DbSet<WorkerDAO> Workers { get; set; }
        public DbSet<WorkerApplicationDAO> WorkerApplications { get; set; }


    }
}
