namespace OnlineLearningPlatform.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Self-referencing relationship: a category may have a parent category
    public int? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }

    // A category can have any number of sub-categories
    public ICollection<Category> SubCategories { get; set; } = new List<Category>();

    // Every course belongs to exactly one category
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
