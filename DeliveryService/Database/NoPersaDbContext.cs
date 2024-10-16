﻿using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models;
using Route = SharedLibrary.Models.Route;

namespace DeliveryService.Database
{
    public class NoPersaDbContext : DbContext
    {
        public NoPersaDbContext(DbContextOptions<NoPersaDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Holiday> Holidays { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                      .ToTable("Customer")
                      .HasKey(c => c.Id);

            modelBuilder.Entity<Route>()
                        .ToTable("Route")
                        .HasKey(r => r.Id);

            modelBuilder.Entity<Holiday>()
                        .ToTable("Holiday")
                        .HasKey(h => h.Id);
        }
    }
}
