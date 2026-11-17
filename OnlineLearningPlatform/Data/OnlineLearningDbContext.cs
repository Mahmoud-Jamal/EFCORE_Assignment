using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineLearningPlatform.Models;

namespace OnlineLearningPlatform.Data;

public class OnlineLearningDbContext : DbContext
{
    public OnlineLearningDbContext()
    {
    }

    public OnlineLearningDbContext(DbContextOptions<OnlineLearningDbContext> options)
        : base(options)
    {
    }

    public DbSet<Instructor> Instructors => Set<Instructor>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection")
                ?? "Server=Mahmoudgamal\\SQLEXPRESS;Database=OnlineLearningPlatform;Trusted_Connection=True;TrustServerCertificate=true;";

            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OnlineLearningDbContext).Assembly);
    }
}
