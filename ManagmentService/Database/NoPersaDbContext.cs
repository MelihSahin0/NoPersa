using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace ManagmentService.Database
{
    public class NoPersaDbContext : DbContext
    {
        public NoPersaDbContext(DbContextOptions<NoPersaDbContext> options)
            : base(options)
        {
            RelationalDatabaseCreator? dbCreater = (RelationalDatabaseCreator?)Database.GetService<IDatabaseCreator>();
            if (dbCreater != null)
            {
                // Create Database 
                if (!dbCreater.CanConnect())
                {
                    dbCreater.Create();
                }

                // Create Tables
                if (!dbCreater.HasTables())
                {
                    dbCreater.CreateTables();
                }
            }
        }

        // Define DbSets for your entities
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherForecast>().HasData(
             new WeatherForecast() { Id = 1, Date = DateOnly.FromDateTime(DateTime.Today), TemperatureC = 10, Summary = "a" },
             new WeatherForecast() { Id = 2, Date = DateOnly.FromDateTime(DateTime.Today), TemperatureC = 20, Summary = "b" },
             new WeatherForecast() { Id = 3, Date = DateOnly.FromDateTime(DateTime.Today), TemperatureC = 30, Summary = "c" });
        }
    }
}
