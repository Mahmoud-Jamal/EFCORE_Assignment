using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.Models;

namespace OnlineLearningPlatform.Data.Configurations;

public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.ToTable("Instructors");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(i => i.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(i => i.Email)
            .IsUnique();

        builder.Property(i => i.Bio)
            .IsRequired(false)
            .HasMaxLength(1000);

        builder.Property(i => i.JoinedDate)
            .IsRequired()
            .HasColumnType("date");

        // Relationships are configured on the dependent side or here
        builder.HasMany(i => i.Courses)
            .WithOne(c => c.Instructor)
            .HasForeignKey(c => c.InstructorId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        // Seed Data: 3 Instructors
        builder.HasData(
            new Instructor
            {
                Id = 1,
                FullName = "Ahmed Ali",
                Email = "ahmed.ali@platform.local",
                Bio = "Principal Software Architect with 15+ years experience in .NET and cloud distributed systems.",
                JoinedDate = new DateOnly(2021, 3, 15)
            },
            new Instructor
            {
                Id = 2,
                FullName = "Mona Hassan",
                Email = "mona.hassan@platform.local",
                Bio = "Lead Data Engineer and SQL Server MVP specializing in relational performance and data warehousing.",
                JoinedDate = new DateOnly(2022, 6, 1)
            },
            new Instructor
            {
                Id = 3,
                FullName = "Tarek Mansour",
                Email = "tarek.mansour@platform.local",
                Bio = null, // Optional bio test
                JoinedDate = new DateOnly(2023, 1, 10)
            }
        );
    }
}
