using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.Models;

namespace OnlineLearningPlatform.Data.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.ToTable("Enrollments", t =>
        {
            t.HasCheckConstraint("CK_Enrollments_Progress", "[Progress] >= 0 AND [Progress] <= 100");
            t.HasCheckConstraint("CK_Enrollments_FinalGrade",
                "([FinalGrade] IS NULL) OR ([FinalGrade] >= 0 AND [FinalGrade] <= 100 AND [Status] = 'Completed')");
        });

        // Composite Primary Key
        builder.HasKey(e => new { e.StudentId, e.CourseId });

        builder.Property(e => e.EnrollmentDate)
            .IsRequired();

        builder.Property(e => e.Progress)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(e => e.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(e => e.FinalGrade)
            .IsRequired(false)
            .HasPrecision(5, 2);

        builder.HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
