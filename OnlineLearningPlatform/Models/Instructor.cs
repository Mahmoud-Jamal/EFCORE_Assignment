namespace OnlineLearningPlatform.Models;

public class Instructor
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public DateOnly JoinedDate { get; set; }

    // Navigation property: an instructor can publish many courses
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
