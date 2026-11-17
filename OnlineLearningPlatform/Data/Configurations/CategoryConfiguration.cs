using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.Models;

namespace OnlineLearningPlatform.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Description)
            .IsRequired(false)
            .HasMaxLength(500);

        // Self-referencing relationship: a category may have a parent category
        // DeleteBehavior.Restrict prevents deleting a parent category that still has sub-categories
        builder.HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        // Category -> Courses relationship
        builder.HasMany(c => c.Courses)
            .WithOne(c => c.Category)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        // Seed Data: 7 Categories forming 3 hierarchy levels
        builder.HasData(
            // Level 1: Top-level roots (ParentCategoryId = null)
            new Category
            {
                Id = 1,
                Name = "Software Development",
                Description = "Software engineering, algorithms, and application development.",
                ParentCategoryId = null
            },
            new Category
            {
                Id = 2,
                Name = "Data Science & AI",
                Description = "Data analysis, machine learning, and artificial intelligence.",
                ParentCategoryId = null
            },

            // Level 2: Sub-categories of Level 1
            new Category
            {
                Id = 3,
                Name = "Backend Engineering",
                Description = "Server-side architectures, APIs, and business logic.",
                ParentCategoryId = 1
            },
            new Category
            {
                Id = 4,
                Name = "Frontend Engineering",
                Description = "Client-side interfaces, modern web frameworks, and UX.",
                ParentCategoryId = 1
            },
            new Category
            {
                Id = 5,
                Name = "Database Systems",
                Description = "Relational modeling, query optimization, and storage.",
                ParentCategoryId = 2
            },

            // Level 3: Sub-categories of Level 2
            new Category
            {
                Id = 6,
                Name = "ASP.NET Core",
                Description = "Enterprise web APIs, microservices, and security with .NET.",
                ParentCategoryId = 3
            },
            new Category
            {
                Id = 7,
                Name = "EF Core & SQL",
                Description = "ORM mastery, migrations, and high-performance querying.",
                ParentCategoryId = 5
            }
        );
    }
}
