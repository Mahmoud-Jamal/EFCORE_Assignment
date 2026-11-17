using OnlineLearningPlatform.Models.Enums;

namespace OnlineLearningPlatform.Models;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public CourseLevel Level { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedDate { get; set; }

    // Foreign Key & Navigation to Instructor
    public int InstructorId { get; set; }
    public Instructor? Instructor { get; set; }

    // Foreign Key & Navigation to Category
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    // Navigation property: a course can have many enrollments
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
