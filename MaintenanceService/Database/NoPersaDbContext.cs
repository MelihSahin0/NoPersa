using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models;
using System.Xml;
using Route = SharedLibrary.Models.Route;

namespace MaintenanceService.Database
{
    public class NoPersaDbContext : DbContext
    {
        public NoPersaDbContext(DbContextOptions<NoPersaDbContext> options) : base(options)
        {
        }

        public DbSet<Maintenance> Maintenance { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Weekday> Weekday { get; set; }
        public DbSet<MonthlyOverview> MonthlyOverview { get; set; }
        public DbSet<DailyOverview> DailyOverview { get; set; }
        public DbSet<Route> Route { get; set; }
        public DbSet<Holiday> Holiday { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Workdays)
                .WithOne()
                .HasForeignKey<Customer>(c => c.WorkdaysId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Holidays)
                .WithOne()
                .HasForeignKey<Customer>(c => c.HolidaysId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.MonthlyOverviews)
                .WithOne(m => m.Customer)
                .HasForeignKey(m => m.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasMany(m => m.DailyOverviews)
                .WithOne(d => d.MonthlyOverview)
                .HasForeignKey(d => d.MonthlyOverviewId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Route)
                .WithMany(r => r.Customers)
                .HasForeignKey(c => c.RouteId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Route>().HasData(
                   new Route
                   {
                       Id = int.MinValue,
                       Name = "Archive",
                       Position = int.MaxValue,
                   }
            );

            modelBuilder.Entity<Maintenance>()
                .Property(e => e.NextDailyDeliverySave)
                .HasColumnType("date");
        }

        public void SeedData()
        {
            if (!Maintenance.Any(m => m.Id == 1))
            {
                Maintenance.Add(new Maintenance
                {
                    Id = 1,
                    NextDailyDeliverySave = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)
                });
                SaveChanges();
            }
        }
    }
}
