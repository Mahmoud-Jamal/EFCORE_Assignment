using OnlineLearningPlatform.Models.Enums;

namespace OnlineLearningPlatform.Models;

public class Enrollment
{
    // Composite Key: StudentId + CourseId
    public int StudentId { get; set; }
    public Student? Student { get; set; }

    public int CourseId { get; set; }
    public Course? Course { get; set; }

    public DateTime EnrollmentDate { get; set; }
    public int Progress { get; set; }
    public EnrollmentStatus Status { get; set; }
    public decimal? FinalGrade { get; set; }
}
