using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models;

namespace GastronomyService.Database
{
    public class NoPersaDbContext : DbContext
    {
        public NoPersaDbContext(DbContextOptions<NoPersaDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomersLightDiet> CustomersLightDiets { get; set; }
        public DbSet<LightDiet> LightDiets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                      .ToTable("Customer")
                      .HasKey(c => c.Id);

            modelBuilder.Entity<CustomersLightDiet>()
                    .ToTable("CustomersLightDiet")
                    .HasKey(cld => new { cld.CustomerId, cld.LightDietId});

            modelBuilder.Entity<LightDiet>()
                    .ToTable("LightDiet")
                    .HasKey(ld => ld.Id);
        }
    }
}