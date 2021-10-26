using Microsoft.EntityFrameworkCore;

namespace SampleApi
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherForecast>()
                .Ignore(a => a.TemperatureF);
        }

        public DbSet<WeatherForecast> WeatherForecast { get; set; }
    }
}
