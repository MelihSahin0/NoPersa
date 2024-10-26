using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models;
using Route = SharedLibrary.Models.Route;

namespace ManagementService.Database
{
    public class NoPersaDbContext : DbContext
    {
        public NoPersaDbContext(DbContextOptions<NoPersaDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Weekday> Weekdays { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<MonthlyOverview> MonthlyOverviews { get; set; }
        public DbSet<DailyOverview> DailyOverviews { get; set; }
        public DbSet<LightDiet> LightDiets { get; set; }
        public DbSet<CustomersLightDiet> CustomersLightDiets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                        .ToTable("Customer")
                        .HasKey(c => c.Id);

            modelBuilder.Entity<Weekday>()
                        .ToTable("Weekday")
                        .HasKey(w => w.Id);

            modelBuilder.Entity<Route>()
                        .ToTable("Route")
                        .HasKey(r => r.Id);

            modelBuilder.Entity<MonthlyOverview>()
                        .ToTable("MonthlyOverview")
                        .HasKey(m => m.Id);

            modelBuilder.Entity<DailyOverview>()
                        .ToTable("DailyOverview")
                        .HasKey(d => d.Id);

            modelBuilder.Entity<LightDiet>()
                        .ToTable("LightDiet")
                        .HasKey(d => d.Id); 

            modelBuilder.Entity<CustomersLightDiet>()
                        .ToTable("CustomersLightDiet")
                        .HasKey(cld => new { cld.CustomerId, cld.LightDietId });
        }
    }
}
