using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Data;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Models.Enums;
using OnlineLearningPlatform.Operations;

namespace OnlineLearningPlatform;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        PrintHeader("ONLINE LEARNING PLATFORM - EF CORE CODE-FIRST SYSTEM");

        using (var setupContext = new OnlineLearningDbContext())
        {
            Console.WriteLine("[Database] Applying any pending migrations...");
            setupContext.Database.Migrate();
            Console.WriteLine("[Database] Schema and catalog seed are up to date.");

            // Part 02: Seeding students and enrollments from code in Program.cs
            SeedStudentsAndEnrollments(setupContext);
        }

        // =====================================================================
        // PART 03: OPERATIONS
        // =====================================================================

        PrintSectionHeader("PART 03 - CREATE OPERATIONS");

        // 1. Add an instructor
        int? newInstructorId = PlatformOperations.AddInstructor(
            fullName: "Dr. Khaled Mostafa",
            email: "khaled.mostafa@platform.local",
            bio: "Enterprise cloud architect and distributed systems researcher."
        );

        // 2. Add a course (valid instructor + valid category)
        int validInstructorId = newInstructorId ?? 1;
        int? newCourseId = PlatformOperations.AddCourse(
            title: "Docker & Kubernetes for .NET Developers",
            description: "Master container orchestration, microservice deployments, and CI/CD.",
            price: 79.99m,
            level: CourseLevel.Intermediate,
            instructorId: validInstructorId,
            categoryId: 3 // Backend Engineering
        );

        // Test AddCourse with invalid instructor / category to demonstrate error handling
        PlatformOperations.AddCourse(
            title: "Invalid Course",
            description: "Should fail gracefully",
            price: 39.99m,
            level: CourseLevel.Beginner,
            instructorId: 99999, // Non-existent
            categoryId: 3
        );

        // 3. Add several students (3 students using one AddRange and one SaveChanges)
        var newStudentsBatch = new List<Student>
        {
            new Student { FullName = "Hassan Mahmoud", Email = "hassan.mahmoud@batch.local", DateOfBirth = new DateOnly(2001, 7, 14) },
            new Student { FullName = "Mariam Samir", Email = "mariam.samir@batch.local", DateOfBirth = new DateOnly(2003, 3, 21) },
            new Student { FullName = "Amr Farouk", Email = "amr.farouk@batch.local", DateOfBirth = new DateOnly(1997, 10, 5) }
        };
        var batchStudentIds = PlatformOperations.AddSeveralStudents(newStudentsBatch);

        // 4. Enroll a student
        // 4a. Successful enrollment into a published course
        if (batchStudentIds.Count > 0)
        {
            PlatformOperations.EnrollStudent(studentId: batchStudentIds[0], courseId: 2);
        }

        // 4b. Refusal case 1: Course is unpublished (Course 7 is unpublished)
        Console.WriteLine("\n[Testing Business Rule] Attempting to enroll in an unpublished course:");
        PlatformOperations.EnrollStudent(studentId: 1, courseId: 7);

        // 4c. Refusal case 2: Duplicate enrollment
        Console.WriteLine("\n[Testing Business Rule] Attempting duplicate enrollment:");
        PlatformOperations.EnrollStudent(studentId: 1, courseId: 1);


        PrintSectionHeader("PART 03 - READ OPERATIONS");

        // 5. Find a course: by Id using Find, then by title using FirstOrDefault (and test 'not found')
        PlatformOperations.FindCourse(courseId: 1, title: "Mastering ASP.NET Core Web APIs");
        PlatformOperations.FindCourse(courseId: 9999, title: "Non-Existent Course Title");

        // 6. Filter courses: priced between two values, ordered by price descending
        PlatformOperations.FilterCourses(minPrice: 50.00m, maxPrice: 100.00m);

        // 7. Sub-categories: direct sub-categories of a category, then top-level categories
        PlatformOperations.PrintCategoriesHierarchy(parentCategoryId: 1);

        // 8. Course roster: student count, then student names (no joins, using Contains)
        PlatformOperations.PrintCourseRoster(courseId: 1);


        PrintSectionHeader("PART 03 - UPDATE OPERATIONS");

        // 9. Change a price: load course, change price, save
        PlatformOperations.ChangePrice(courseId: 1, newPrice: 54.99m);

        // 10. Update progress: find enrollment by composite key, set progress
        // 10a. Valid update reaching 100 -> status switches to Completed
        PlatformOperations.UpdateProgress(studentId: 1, courseId: 2, newProgress: 100, finalGrade: 96.50m);

        // 10b. Rejection test: progress > 100
        PlatformOperations.UpdateProgress(studentId: 1, courseId: 2, newProgress: 125);

        // 11. Publish a course: set published flag and published date
        PlatformOperations.PublishCourse(courseId: 3); // Course 3 was seeded as unpublished


        PrintSectionHeader("PART 03 - DELETE OPERATIONS");

        // 12. Unenroll: delete an enrollment using composite key
        PlatformOperations.Unenroll(studentId: 3, courseId: 5);

        // 13. Delete a category:
        // 13a. Unsafe attempt (catches DbUpdateException)
        PlatformOperations.DeleteCategoryUnsafe(categoryId: 1); // Has sub-categories 3 & 4

        // 13b. Safe version checks for sub-categories before deleting
        PlatformOperations.DeleteCategorySafe(categoryId: 1);

        // 13c. Safe deletion of an isolated leaf category without sub-categories or courses
        int demoCatId = CreateDemoLeafCategory();
        PlatformOperations.DeleteCategorySafe(categoryId: demoCatId);

        // 14. Delete a course: delete a course that has enrollments, print before/after count & explanation
        int demoCourseId = CreateDemoCourseWithEnrollment();
        PlatformOperations.DeleteCourse(courseId: demoCourseId);


        PrintSectionHeader("PART 03 - BULK OPERATIONS");

        // 15. Bulk price raise: raise price of all published Intermediate courses by 10% in single statement
        PlatformOperations.BulkPriceRaise(level: CourseLevel.Intermediate, percentage: 0.10m);

        // 16. Bulk delete: delete all enrollments whose status is Dropped in single statement
        PlatformOperations.BulkDeleteDroppedEnrollments();

        PrintHeader("ALL OPERATIONS COMPLETED SUCCESSFULLY!");
    }

    /// <summary>
    /// Part 02: Seeds at least 5 students and 8 enrollments covering all three statuses (Active, Completed, Dropped).
    /// Only seeds when tables are empty to ensure idempotency.
    /// </summary>
    private static void SeedStudentsAndEnrollments(OnlineLearningDbContext context)
    {
        Console.WriteLine("\n--- [PART 02: SEEDING FROM CODE IN PROGRAM.CS] ---");

        if (context.Students.Any() || context.Enrollments.Any())
        {
            Console.WriteLine("[Seeding] Students and Enrollments already exist in the database. Skipping duplicate code seed.");
            return;
        }

        Console.WriteLine("[Seeding] Seeding 5 initial Students and 8 initial Enrollments across all statuses...");

        // 1. Seed 5 Students
        var initialStudents = new List<Student>
        {
            new Student { FullName = "Youssef Ibrahim", Email = "youssef.ibrahim@student.local", DateOfBirth = new DateOnly(2001, 5, 12) },
            new Student { FullName = "Sarah Nabil", Email = "sarah.nabil@student.local", DateOfBirth = new DateOnly(1999, 11, 23) },
            new Student { FullName = "Omar Khaled", Email = "omar.khaled@student.local", DateOfBirth = new DateOnly(2002, 8, 4) },
            new Student { FullName = "Nouran Adel", Email = "nouran.adel@student.local", DateOfBirth = new DateOnly(2000, 2, 17) },
            new Student { FullName = "Karim Fouad", Email = "karim.fouad@student.local", DateOfBirth = new DateOnly(1998, 9, 30) }
        };

        context.Students.AddRange(initialStudents);
        context.SaveChanges();

        // Retrieve generated student IDs
        int s1 = initialStudents[0].Id;
        int s2 = initialStudents[1].Id;
        int s3 = initialStudents[2].Id;
        int s4 = initialStudents[3].Id;
        int s5 = initialStudents[4].Id;

        // 2. Seed 8 Enrollments across published courses (Courses 1, 2, 4, 5, 6)
        // Covering Active, Completed, Dropped
        var initialEnrollments = new List<Enrollment>
        {
            // Completed enrollments (Progress = 100, FinalGrade populated)
            new Enrollment { StudentId = s1, CourseId = 1, EnrollmentDate = DateTime.UtcNow.AddDays(-60), Progress = 100, Status = EnrollmentStatus.Completed, FinalGrade = 95.50m },
            new Enrollment { StudentId = s2, CourseId = 1, EnrollmentDate = DateTime.UtcNow.AddDays(-45), Progress = 100, Status = EnrollmentStatus.Completed, FinalGrade = 88.00m },
            new Enrollment { StudentId = s3, CourseId = 5, EnrollmentDate = DateTime.UtcNow.AddDays(-40), Progress = 100, Status = EnrollmentStatus.Completed, FinalGrade = 92.00m },

            // Active enrollments (Progress < 100, FinalGrade is null)
            new Enrollment { StudentId = s1, CourseId = 2, EnrollmentDate = DateTime.UtcNow.AddDays(-30), Progress = 45, Status = EnrollmentStatus.Active, FinalGrade = null },
            new Enrollment { StudentId = s3, CourseId = 2, EnrollmentDate = DateTime.UtcNow.AddDays(-15), Progress = 60, Status = EnrollmentStatus.Active, FinalGrade = null },
            new Enrollment { StudentId = s4, CourseId = 4, EnrollmentDate = DateTime.UtcNow.AddDays(-10), Progress = 80, Status = EnrollmentStatus.Active, FinalGrade = null },

            // Dropped enrollments (Progress < 100, FinalGrade is null)
            new Enrollment { StudentId = s2, CourseId = 4, EnrollmentDate = DateTime.UtcNow.AddDays(-20), Progress = 15, Status = EnrollmentStatus.Dropped, FinalGrade = null },
            new Enrollment { StudentId = s5, CourseId = 6, EnrollmentDate = DateTime.UtcNow.AddDays(-5), Progress = 10, Status = EnrollmentStatus.Dropped, FinalGrade = null }
        };

        context.Enrollments.AddRange(initialEnrollments);
        context.SaveChanges();

        Console.WriteLine($"[Seeding Complete] Successfully inserted {initialStudents.Count} students and {initialEnrollments.Count} enrollments.");
    }

    /// <summary>
    /// Helper to create an isolated demo category with no children or courses for testing safe deletion.
    /// </summary>
    private static int CreateDemoLeafCategory()
    {
        using var context = new OnlineLearningDbContext();
        var tempCat = new Category
        {
            Name = "Temporary Leaf Category " + Guid.NewGuid().ToString()[..6],
            Description = "Created specifically to test safe category deletion.",
            ParentCategoryId = null
        };
        context.Categories.Add(tempCat);
        context.SaveChanges();
        return tempCat.Id;
    }

    /// <summary>
    /// Helper to create an isolated course with enrollments to test cascade deletion without wiping main seed data.
    /// </summary>
    private static int CreateDemoCourseWithEnrollment()
    {
        using var context = new OnlineLearningDbContext();
        var tempCourse = new Course
        {
            Title = "Temporary Cascade Demo Course " + Guid.NewGuid().ToString()[..6],
            Description = "Created specifically to verify cascade deletion on enrollments.",
            Price = 29.99m,
            Level = CourseLevel.Beginner,
            IsPublished = true,
            PublishedDate = DateTime.UtcNow,
            InstructorId = 1,
            CategoryId = 1
        };
        context.Courses.Add(tempCourse);
        context.SaveChanges();

        // Add 2 temporary enrollments for this course
        var student = context.Students.First();
        var tempEnrollment = new Enrollment
        {
            StudentId = student.Id,
            CourseId = tempCourse.Id,
            EnrollmentDate = DateTime.UtcNow,
            Progress = 20,
            Status = EnrollmentStatus.Active
        };
        context.Enrollments.Add(tempEnrollment);
        context.SaveChanges();

        return tempCourse.Id;
    }

    private static void PrintHeader(string title)
    {
        Console.WriteLine("\n" + new string('=', 80));
        Console.WriteLine($"  {title}");
        Console.WriteLine(new string('=', 80));
    }

    private static void PrintSectionHeader(string section)
    {
        Console.WriteLine("\n" + new string('#', 80));
        Console.WriteLine($"  {section}");
        Console.WriteLine(new string('#', 80));
    }
}
