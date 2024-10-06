using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models;

namespace ManagmentService.Database
{
    public class NoPersaDbContext : DbContext
    {
        public NoPersaDbContext(DbContextOptions<NoPersaDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Weekdays> Weekdays { get; set; }
        public DbSet<MonthlyOverview> MonthlyOverview { get; set; }
        public DbSet<DailyOverview> DailyOverview { get; set; }

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
                .HasOne(m => m.Day1)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day1Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day2)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day2Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day3)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day3Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day4)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day4Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day5)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day5Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day6)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day6Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day7)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day7Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day8)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day8Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day9)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day9Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day10)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day10Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day11)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day11Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day12)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day12Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day13)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day13Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day14)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day14Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day15)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day15Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day16)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day16Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day17)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day17Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day18)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day18Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day19)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day19Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day20)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day20Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day21)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day21Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day22)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day22Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day23)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day23Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day24)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day24Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day25)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day25Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day26)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day26Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day27)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day27Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day28)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day28Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day29)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day29Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day30)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day30Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MonthlyOverview>()
                .HasOne(m => m.Day31)
                .WithOne()
                .HasForeignKey<MonthlyOverview>(m => m.Day31Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Route)
                .WithMany(r => r.Customers)
                .HasForeignKey(c => c.RouteId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
