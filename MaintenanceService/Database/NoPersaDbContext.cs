using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models;
using SharedLibrary.Util;
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
        public DbSet<Article> Article { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Maintenance>()
                .Property(e => e.Date)
                .HasColumnType("date");

            modelBuilder.Entity<Customer>()
                .HasOne(dl => dl.DeliveryLocation)
                .WithOne(c => c.Customer)
                .HasForeignKey<DeliveryLocation>(dl => dl.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Workdays)
                .WithOne()
                .HasForeignKey<Customer>(c => c.WorkdaysId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Holidays)
                .WithOne()
                .HasForeignKey<Customer>(c => c.HolidaysId)
                .OnDelete(DeleteBehavior.Cascade);

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

            modelBuilder.Entity<Customer>()
                .HasOne(a => a.Article)
                .WithMany(c => c.Customers)
                .HasForeignKey(c => c.ArticleId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.LightDiets)
                .WithMany(ld => ld.Customers)
                .UsingEntity<CustomersLightDiet>(
                    i => i
                        .HasOne<LightDiet>(cld => cld.LightDiet)
                        .WithMany(ld => ld.CustomersLightDiets)
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<Customer>(cld => cld.Customer)
                        .WithMany(c => c.CustomersLightDiets)
                        .OnDelete(DeleteBehavior.Restrict)
                );

            modelBuilder.Entity<CustomersMenuPlan>()
                   .HasKey(cmp => new { cmp.CustomerId, cmp.BoxContentId });

            modelBuilder.Entity<CustomersMenuPlan>()
                .HasOne(cmp => cmp.Customer)
                .WithMany(c => c.CustomerMenuPlans)
                .HasForeignKey(cmp => cmp.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CustomersMenuPlan>()
                .HasOne(cmp => cmp.BoxContent)
                .WithMany(bc => bc.CustomerMenuPlans)
                .HasForeignKey(cmp => cmp.BoxContentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CustomersMenuPlan>()
                .HasOne(cmp => cmp.PortionSize)
                .WithMany(ps => ps.CustomerMenuPlans)
                .HasForeignKey(cmp => cmp.PortionSizeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.FoodWishes)
                .WithMany(ld => ld.Customers)
                .UsingEntity<CustomersFoodWish>(
                    i => i
                        .HasOne<FoodWish>(cld => cld.FoodWish)
                        .WithMany(ld => ld.CustomersFoodWishes)
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<Customer>(cld => cld.Customer)
                        .WithMany(c => c.CustomersFoodWish)
                        .OnDelete(DeleteBehavior.Restrict)
                );
        }

        public void SeedData()
        {
            if (!Route.Any(r => r.Id == int.MinValue))
            {
                Route.Add(new Route
                {
                    Id = int.MinValue,
                    Name = "Archive",
                    Position = int.MaxValue,
                });
                SaveChanges();
            }

            if (!Maintenance.Any(m => m.Id == 1))
            {
                Maintenance.Add(new Maintenance
                {
                    Id = 1,
                    Type = MaintenanceTypes.DailyDelivery.ToString(),
                    Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)
                });
                SaveChanges();
            }
        }
    }
}
