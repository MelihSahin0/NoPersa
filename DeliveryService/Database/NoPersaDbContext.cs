using Microsoft.EntityFrameworkCore;
using Route = SharedLibrary.Models.Route;

namespace DeliveryService.Database
{
    public class NoPersaDbContext : DbContext
    {
        public NoPersaDbContext(DbContextOptions<NoPersaDbContext> options) : base(options)
        {
        }

        public DbSet<Route> Routes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
