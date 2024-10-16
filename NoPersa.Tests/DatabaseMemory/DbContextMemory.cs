using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models;

namespace NoPersa.Tests.DatabaseMemory
{
    public class DbContextMemory : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DailyOverview> DailyOverviews { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<MonthlyOverview> MonthlyOverviews { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Weekday> Weekdays { get; set; }

        public DbContextMemory(DbContextOptions<DbContextMemory> options) : base(options)
        {
        }
    }
}
