using EventHub.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHub.Data
{
    public class EventHubDbContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=Mahmoudgamal\\SQLEXPRESS;Database=EventHub;Trusted_Connection=True;TrustServerCertificate=true;");


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventHubDbContext).Assembly);

        }

        public DbSet<Organizer> Organizers => Set<Organizer>();
        public DbSet<Attendee> Attendees => Set<Attendee>();


    }
}
