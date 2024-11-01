using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models;

namespace GastronomyService.Database
{
    public class NoPersaDbContext : DbContext
    {
        public NoPersaDbContext(DbContextOptions<NoPersaDbContext> options) : base(options)
        {
        }

        public DbSet<CustomersLightDiet> CustomersLightDiets { get; set; }
        public DbSet<CustomersMenuPlan> CustomersMenuPlans { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<LightDiet> LightDiets { get; set; }
        public DbSet<BoxContent> BoxContents { get; set; }
        public DbSet<PortionSize> PortionSizes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomersLightDiet>()
                    .ToTable("CustomersLightDiet")
                    .HasKey(cld => new { cld.CustomerId, cld.LightDietId});

            modelBuilder.Entity<CustomersMenuPlan>()
                    .ToTable("CustomersMenuPlan")
                    .HasKey(cmp => new { cmp.CustomerId, cmp.BoxContentId });

            modelBuilder.Entity<Customer>()
                    .ToTable("Customer")
                    .HasKey(c => c.Id);

            modelBuilder.Entity<LightDiet>()
                    .ToTable("LightDiet")
                    .HasKey(ld => ld.Id);

            modelBuilder.Entity<BoxContent>()
                    .ToTable("BoxContent")
                    .HasKey(b => b.Id);

            modelBuilder.Entity<PortionSize>()
                    .ToTable("PortionSize")
                    .HasKey(p => p.Id);
        }
    }
}