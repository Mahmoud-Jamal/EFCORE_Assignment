using EFCOREExercise.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCOREExercise
{
    internal class libraryDbContext : DbContext


    {
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=Mahmoudgamal\\SQLEXPRESS;Database=BOOKSTORE;Trusted_Connection=True;TrustServerCertificate=true;");
            

        }
        public DbSet<book> Books { get; set; }
        public DbSet<author> Authors { get; set; }

        public DbSet<category> Categories { get; set; }
    }
}
