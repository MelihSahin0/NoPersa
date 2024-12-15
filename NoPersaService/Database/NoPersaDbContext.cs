using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.FluentValidations;
using SharedLibrary.Models;
using SharedLibrary.Util;
using System.ComponentModel.DataAnnotations;
using Route = SharedLibrary.Models.Route;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace NoPersaService.Database
{
    public class NoPersaDbContext : DbContext
    {
        private readonly IServiceProvider serviceProvider;

        public NoPersaDbContext(DbContextOptions<NoPersaDbContext> options, IServiceProvider serviceProvider) : base(options)
        {
            this.serviceProvider = serviceProvider;
        }

        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Weekday> Weekdays { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<MonthlyOverview> MonthlyOverviews { get; set; }
        public DbSet<DailyOverview> DailyOverviews { get; set; }
        public DbSet<DeliveryLocation> DeliveryLocations { get; set; }
        public DbSet<LightDiet> LightDiets { get; set; }
        public DbSet<BoxContent> BoxContents { get; set; }
        public DbSet<PortionSize> PortionSizes { get; set; }
        public DbSet<FoodWish> FoodWishes { get; set; }
        public DbSet<CustomersLightDiet> CustomersLightDiets { get; set; }
        public DbSet<CustomersMenuPlan> CustomersMenuPlans { get; set; }
        public DbSet<CustomersFoodWish> CustomersFoodWishes { get; set; }
        public DbSet<BoxStatus> BoxStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region relation
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
            #endregion

            #region tableToUse
            modelBuilder.Entity<Maintenance>().ToTable("Maintenance");
            modelBuilder.Entity<Holiday>().ToTable("Holiday");
            modelBuilder.Entity<Route>().ToTable("Route");
            modelBuilder.Entity<Article>().ToTable("Article");
            modelBuilder.Entity<Weekday>().ToTable("Weekday");
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<MonthlyOverview>().ToTable("MonthlyOverview");
            modelBuilder.Entity<DailyOverview>().ToTable("DailyOverview");
            modelBuilder.Entity<DeliveryLocation>().ToTable("DeliveryLocation");
            modelBuilder.Entity<LightDiet>().ToTable("LightDiet");
            modelBuilder.Entity<BoxContent>().ToTable("BoxContent");
            modelBuilder.Entity<PortionSize>().ToTable("PortionSize");
            modelBuilder.Entity<FoodWish>().ToTable("FoodWish");
            modelBuilder.Entity<CustomersLightDiet>().ToTable("CustomersLightDiet");
            modelBuilder.Entity<CustomersMenuPlan>().ToTable("CustomersMenuPlan");
            modelBuilder.Entity<CustomersFoodWish>().ToTable("CustomersFoodWish");
            modelBuilder.Entity<BoxStatus>().ToTable("BoxStatus");
            #endregion
        }

        public override int SaveChanges()
        {
            ValidateEntities();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ValidateEntities();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ValidateEntities()
        {
            var entities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(e => e.Entity);

            foreach (var entity in entities)
            {
                ValidateEntity(entity);
            }
        }

        private void ValidateEntity(object entity)
        {
            var validator = serviceProvider.GetService(typeof(IValidator<>).MakeGenericType(entity.GetType())) as IValidator;
           
            if (validator != null)
            {
                var context = new ValidationContext<object>(entity);
                var validationResult = validator.Validate(context);

                if (!validationResult.IsValid)
                {
                    var errorMessages = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    throw new ValidationException($"Validation failed for {entity.GetType().Name}: {errorMessages}");
                }
            }
        }

        public void SeedData()
        {
            if (!Routes.Any(r => r.Id == long.MinValue))
            {
                Routes.Add(new Route
                {
                    Id = long.MinValue,
                    Name = "Archive",
                    Position = int.MaxValue,
                    IsDrivable = false
                });
                SaveChanges();
            }

            if (!Maintenances.Any(m => m.Id == 1))
            {
                Maintenances.Add(new Maintenance
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
