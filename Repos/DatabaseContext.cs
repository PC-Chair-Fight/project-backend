using Microsoft.EntityFrameworkCore;

namespace project_backend.Repos
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<WeatherForecast> Forecasts { get; set; }
    }
}
