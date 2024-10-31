using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models;

namespace GastronomyService.Database
{
    public class NoPersaDbContext : DbContext
    {
        public NoPersaDbContext(DbContextOptions<NoPersaDbContext> options) : base(options)
        {
        }

        public DbSet<LightDiet> LightDiets { get; set; }
        public DbSet<BoxContent> BoxContents { get; set; }
        public DbSet<PortionSize> PortionSizes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomersLightDiet>()
                    .ToTable("CustomersLightDiet")
                    .HasKey(cld => new { cld.CustomerId, cld.LightDietId});

            modelBuilder.Entity<LightDiet>()
                    .ToTable("LightDiet")
                    .HasKey(ld => ld.Id);

            modelBuilder.Entity<BoxContent>()
                    .ToTable("BoxContent")
                    .HasKey(b => b.Id);

            modelBuilder.Entity<PortionSize>()
                    .ToTable("PortionSize")
                    .HasKey(p => p.Id);

            modelBuilder.Entity<CustomersMenuPlan>()
                    .ToTable("CustomersMenuPlan")
                    .HasKey(cmp => new { cmp.CustomerId, cmp.BoxContentId });
        }
    }
}