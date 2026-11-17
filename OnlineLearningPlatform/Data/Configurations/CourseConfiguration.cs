using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Models.Enums;

namespace OnlineLearningPlatform.Data.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses", t =>
        {
            t.HasCheckConstraint("CK_Courses_Price", "[Price] >= 0");
            t.HasCheckConstraint("CK_Courses_PublishedDate",
                "([IsPublished] = 0 AND [PublishedDate] IS NULL) OR ([IsPublished] = 1 AND [PublishedDate] IS NOT NULL)");
        });

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Description)
            .IsRequired(false)
            .HasMaxLength(2000);

        builder.Property(c => c.Price)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(c => c.Level)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(c => c.IsPublished)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(c => c.PublishedDate)
            .IsRequired(false);

        // One-to-Many: Instructor <-> Course
        builder.HasOne(c => c.Instructor)
            .WithMany(i => i.Courses)
            .HasForeignKey(c => c.InstructorId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        // One-to-Many: Category <-> Course
        builder.HasOne(c => c.Category)
            .WithMany(cat => cat.Courses)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        // Course <-> Enrollments (Cascade delete enrollments when course is deleted)
        builder.HasMany(c => c.Enrollments)
            .WithOne(e => e.Course)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        // Seed Data: 7 Courses (mixture of published and unpublished, spread across instructors, categories, and levels)
        builder.HasData(
            new Course
            {
                Id = 1,
                Title = "C# Fundamentals for Beginners",
                Description = "Comprehensive introduction to C# syntax, OOP, and modern language features.",
                Price = 49.99m,
                Level = CourseLevel.Beginner,
                IsPublished = true,
                PublishedDate = new DateTime(2024, 1, 15, 10, 0, 0, DateTimeKind.Utc),
                InstructorId = 1,
                CategoryId = 1 // Root: Software Development
            },
            new Course
            {
                Id = 2,
                Title = "Mastering ASP.NET Core Web APIs",
                Description = "Production-ready RESTful APIs, dependency injection, and authentication.",
                Price = 89.99m,
                Level = CourseLevel.Intermediate,
                IsPublished = true,
                PublishedDate = new DateTime(2024, 2, 20, 14, 0, 0, DateTimeKind.Utc),
                InstructorId = 1,
                CategoryId = 6 // Level 3: ASP.NET Core
            },
            new Course
            {
                Id = 3,
                Title = "Modern Angular & Micro-Frontends",
                Description = "Building scalable single page applications with Angular and TypeScript.",
                Price = 69.99m,
                Level = CourseLevel.Intermediate,
                IsPublished = false,
                PublishedDate = null,
                InstructorId = 3,
                CategoryId = 4 // Level 2: Frontend Engineering
            },
            new Course
            {
                Id = 4,
                Title = "Relational Database Design with SQL Server",
                Description = "Normalization, indexing, transaction management, and stored procedures.",
                Price = 59.99m,
                Level = CourseLevel.Beginner,
                IsPublished = true,
                PublishedDate = new DateTime(2024, 3, 10, 9, 30, 0, DateTimeKind.Utc),
                InstructorId = 2,
                CategoryId = 5 // Level 2: Database Systems
            },
            new Course
            {
                Id = 5,
                Title = "High-Performance EF Core 10",
                Description = "Advanced query tuning, compiled models, concurrency handling, and bulk operations.",
                Price = 99.99m,
                Level = CourseLevel.Advanced,
                IsPublished = true,
                PublishedDate = new DateTime(2024, 4, 5, 16, 0, 0, DateTimeKind.Utc),
                InstructorId = 2,
                CategoryId = 7 // Level 3: EF Core & SQL
            },
            new Course
            {
                Id = 6,
                Title = "Cloud-Native Microservices Architecture",
                Description = "Event-driven design, message brokers, containerization, and resilience patterns.",
                Price = 119.99m,
                Level = CourseLevel.Advanced,
                IsPublished = true,
                PublishedDate = new DateTime(2024, 5, 1, 11, 0, 0, DateTimeKind.Utc),
                InstructorId = 1,
                CategoryId = 3 // Level 2: Backend Engineering
            },
            new Course
            {
                Id = 7,
                Title = "Generative AI Engineering with Python & C#",
                Description = "Integrating LLMs, semantic kernel, and vector embeddings in enterprise apps.",
                Price = 129.99m,
                Level = CourseLevel.Advanced,
                IsPublished = false,
                PublishedDate = null,
                InstructorId = 3,
                CategoryId = 2 // Root: Data Science & AI
            }
        );
    }
}
