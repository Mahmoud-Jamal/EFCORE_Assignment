using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Data;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Models.Enums;

namespace OnlineLearningPlatform.Operations;

public static class PlatformOperations
{
    // =========================================================================
    // CREATE OPERATIONS
    // =========================================================================

    /// <summary>
    /// Inserts a new instructor and prints the generated Id.
    /// </summary>
    public static int? AddInstructor(string fullName, string email, string? bio)
    {
        Console.WriteLine($"\n--- [CREATE] Add Instructor: {fullName} ---");
        using var context = new OnlineLearningDbContext();

        if (context.Instructors.Any(i => i.Email == email))
        {
            var existing = context.Instructors.First(i => i.Email == email);
            Console.WriteLine($"[Info] Instructor with email '{email}' already exists (Id: {existing.Id}).");
            return existing.Id;
        }

        var instructor = new Instructor
        {
            FullName = fullName,
            Email = email,
            Bio = bio,
            JoinedDate = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        context.Instructors.Add(instructor);
        context.SaveChanges();

        Console.WriteLine($"[Success] Instructor '{instructor.FullName}' inserted successfully with generated Id: {instructor.Id}");
        return instructor.Id;
    }

    /// <summary>
    /// Inserts a course for an existing instructor and an existing category.
    /// Checks that both exist first, otherwise prints a friendly message.
    /// </summary>
    public static int? AddCourse(string title, string? description, decimal price, CourseLevel level, int instructorId, int categoryId)
    {
        Console.WriteLine($"\n--- [CREATE] Add Course: '{title}' ---");
        using var context = new OnlineLearningDbContext();

        // 1. Verify Instructor exists
        var instructorExists = context.Instructors.Any(i => i.Id == instructorId);
        if (!instructorExists)
        {
            Console.WriteLine($"[Error] Cannot add course. Instructor with Id {instructorId} does not exist.");
            return null;
        }

        // 2. Verify Category exists
        var categoryExists = context.Categories.Any(c => c.Id == categoryId);
        if (!categoryExists)
        {
            Console.WriteLine($"[Error] Cannot add course. Category with Id {categoryId} does not exist.");
            return null;
        }

        var course = new Course
        {
            Title = title,
            Description = description,
            Price = price,
            Level = level,
            IsPublished = false,
            PublishedDate = null,
            InstructorId = instructorId,
            CategoryId = categoryId
        };

        context.Courses.Add(course);
        context.SaveChanges();

        Console.WriteLine($"[Success] Course '{course.Title}' created successfully with generated Id: {course.Id} (Unpublished, Price: ${course.Price:F2})");
        return course.Id;
    }

    /// <summary>
    /// Inserts three students using one AddRange and one SaveChanges. Prints their generated Ids.
    /// </summary>
    public static List<int> AddSeveralStudents(List<Student> students)
    {
        Console.WriteLine($"\n--- [CREATE] Add Several Students ({students.Count} students) via single AddRange ---");
        using var context = new OnlineLearningDbContext();

        // Filter out any student whose email is already registered to ensure idempotency
        var existingEmails = context.Students.Select(s => s.Email).ToHashSet();
        var studentsToAdd = students.Where(s => !existingEmails.Contains(s.Email)).ToList();

        if (studentsToAdd.Count == 0)
        {
            Console.WriteLine("[Info] All provided students are already registered in the database.");
            return context.Students.Where(s => students.Select(st => st.Email).Contains(s.Email)).Select(s => s.Id).ToList();
        }

        // Single AddRange call
        context.Students.AddRange(studentsToAdd);
        // Single SaveChanges call
        context.SaveChanges();

        Console.WriteLine($"[Success] {studentsToAdd.Count} students inserted in a single batch:");
        foreach (var s in studentsToAdd)
        {
            Console.WriteLine($"  - Id: {s.Id} | Name: {s.FullName} | Email: {s.Email}");
        }

        return studentsToAdd.Select(s => s.Id).ToList();
    }

    /// <summary>
    /// Enrolls a student in a course.
    /// Validates student exists, course exists, course is published, and student is not enrolled already.
    /// Prints why the enrollment was refused if any condition fails.
    /// </summary>
    public static bool EnrollStudent(int studentId, int courseId)
    {
        Console.WriteLine($"\n--- [CREATE] Enroll Student {studentId} into Course {courseId} ---");
        using var context = new OnlineLearningDbContext();

        // 1. Check Student exists
        var student = context.Students.Find(studentId);
        if (student == null)
        {
            Console.WriteLine($"[Refusal] Enrollment refused: Student with Id {studentId} was not found.");
            return false;
        }

        // 2. Check Course exists
        var course = context.Courses.Find(courseId);
        if (course == null)
        {
            Console.WriteLine($"[Refusal] Enrollment refused: Course with Id {courseId} was not found.");
            return false;
        }

        // 3. Check Course is published
        if (!course.IsPublished)
        {
            Console.WriteLine($"[Refusal] Enrollment refused: Course '{course.Title}' (Id: {courseId}) is NOT published. Only published courses accept enrollments.");
            return false;
        }

        // 4. Check if already enrolled (composite key lookup)
        var existingEnrollment = context.Enrollments.Find(studentId, courseId);
        if (existingEnrollment != null)
        {
            Console.WriteLine($"[Refusal] Enrollment refused: Student '{student.FullName}' is ALREADY enrolled in course '{course.Title}'.");
            return false;
        }

        // Create new enrollment
        var enrollment = new Enrollment
        {
            StudentId = studentId,
            CourseId = courseId,
            EnrollmentDate = DateTime.UtcNow,
            Progress = 0,
            Status = EnrollmentStatus.Active,
            FinalGrade = null
        };

        context.Enrollments.Add(enrollment);
        context.SaveChanges();

        Console.WriteLine($"[Success] Student '{student.FullName}' successfully enrolled in '{course.Title}' (Status: Active, Progress: 0%).");
        return true;
    }

    // =========================================================================
    // READ OPERATIONS
    // =========================================================================

    /// <summary>
    /// Gets a course by Id using Find, then by title using FirstOrDefault.
    /// Prints "not found" when the result is null.
    /// </summary>
    public static void FindCourse(int courseId, string title)
    {
        Console.WriteLine($"\n--- [READ] Find Course (by Id: {courseId} & by Title: '{title}') ---");
        using var context = new OnlineLearningDbContext();

        // Lookup by Id using Find
        Console.WriteLine($"Executing context.Courses.Find({courseId})...");
        var courseById = context.Courses.Find(courseId);
        if (courseById != null)
        {
            Console.WriteLine($"  [Found by Id] Id: {courseById.Id} | Title: {courseById.Title} | Price: ${courseById.Price:F2} | Published: {courseById.IsPublished}");
        }
        else
        {
            Console.WriteLine($"  [Not Found] Course with Id {courseId} was not found.");
        }

        // Lookup by Title using FirstOrDefault
        Console.WriteLine($"Executing context.Courses.FirstOrDefault(c => c.Title == '{title}')...");
        var courseByTitle = context.Courses.FirstOrDefault(c => c.Title == title);
        if (courseByTitle != null)
        {
            Console.WriteLine($"  [Found by Title] Id: {courseByTitle.Id} | Title: {courseByTitle.Title} | Level: {courseByTitle.Level} | Price: ${courseByTitle.Price:F2}");
        }
        else
        {
            Console.WriteLine($"  [Not Found] Course with title '{title}' was not found.");
        }
    }

    /// <summary>
    /// Courses priced between two values, ordered by price descending.
    /// Prints only the title, level, and price.
    /// </summary>
    public static void FilterCourses(decimal minPrice, decimal maxPrice)
    {
        Console.WriteLine($"\n--- [READ] Filter Courses Priced Between ${minPrice:F2} and ${maxPrice:F2} (Descending) ---");
        using var context = new OnlineLearningDbContext();

        if (minPrice > maxPrice)
        {
            Console.WriteLine("[Error] Minimum price cannot be greater than maximum price.");
            return;
        }

        var results = context.Courses
            .Where(c => c.Price >= minPrice && c.Price <= maxPrice)
            .OrderByDescending(c => c.Price)
            .Select(c => new
            {
                c.Title,
                c.Level,
                c.Price
            })
            .ToList();

        if (results.Count == 0)
        {
            Console.WriteLine($"  No courses found in the range ${minPrice:F2} - ${maxPrice:F2}.");
            return;
        }

        Console.WriteLine($"Found {results.Count} matching courses:");
        foreach (var c in results)
        {
            Console.WriteLine($"  - Title: {c.Title,-45} | Level: {c.Level,-12} | Price: ${c.Price,6:F2}");
        }
    }

    /// <summary>
    /// Prints the direct sub-categories of a given category, then prints the top-level categories.
    /// </summary>
    public static void PrintCategoriesHierarchy(int parentCategoryId)
    {
        Console.WriteLine($"\n--- [READ] Sub-Categories for Parent Id {parentCategoryId} & All Top-Level Categories ---");
        using var context = new OnlineLearningDbContext();

        var parentCat = context.Categories.Find(parentCategoryId);
        var parentName = parentCat?.Name ?? $"Unknown (Id: {parentCategoryId})";

        // Direct sub-categories of the given category
        var subCategories = context.Categories
            .Where(c => c.ParentCategoryId == parentCategoryId)
            .Select(c => new { c.Id, c.Name, c.Description })
            .ToList();

        Console.WriteLine($"Direct Sub-Categories of '{parentName}' (Id: {parentCategoryId}):");
        if (subCategories.Count == 0)
        {
            Console.WriteLine("  (No direct sub-categories found)");
        }
        else
        {
            foreach (var sub in subCategories)
            {
                Console.WriteLine($"  * Id: {sub.Id} | Name: {sub.Name} | Description: {sub.Description}");
            }
        }

        // Top-level categories (no parent)
        var topLevelCategories = context.Categories
            .Where(c => c.ParentCategoryId == null)
            .Select(c => new { c.Id, c.Name, c.Description })
            .ToList();

        Console.WriteLine("\nTop-Level Categories (ParentCategoryId IS NULL):");
        foreach (var root in topLevelCategories)
        {
            Console.WriteLine($"  * Id: {root.Id} | Name: {root.Name} | Description: {root.Description}");
        }
    }

    /// <summary>
    /// Prints how many students are enrolled in a course, then their names.
    /// Per assignment: done without joins: get student Ids from enrollments first, then get students with Contains.
    /// </summary>
    public static void PrintCourseRoster(int courseId)
    {
        Console.WriteLine($"\n--- [READ] Course Roster for Course Id {courseId} (Without Joins) ---");
        using var context = new OnlineLearningDbContext();

        var course = context.Courses.Find(courseId);
        if (course == null)
        {
            Console.WriteLine($"[Error] Course with Id {courseId} was not found.");
            return;
        }

        // Step 1: Query enrollments directly to get student Ids
        var studentIds = context.Enrollments
            .Where(e => e.CourseId == courseId)
            .Select(e => e.StudentId)
            .ToList();

        Console.WriteLine($"Course: '{course.Title}' (Id: {courseId})");
        Console.WriteLine($"Enrolled Students Count: {studentIds.Count}");

        if (studentIds.Count == 0)
        {
            Console.WriteLine("  (No students are currently enrolled in this course)");
            return;
        }

        // Step 2: Query students table directly using Contains
        var studentNames = context.Students
            .Where(s => studentIds.Contains(s.Id))
            .Select(s => s.FullName)
            .ToList();

        Console.WriteLine("Student Names:");
        foreach (var name in studentNames)
        {
            Console.WriteLine($"  - {name}");
        }
    }

    // =========================================================================
    // UPDATE OPERATIONS
    // =========================================================================

    /// <summary>
    /// Loads a course, changes its price, and saves.
    /// </summary>
    public static bool ChangePrice(int courseId, decimal newPrice)
    {
        Console.WriteLine($"\n--- [UPDATE] Change Price of Course Id {courseId} to ${newPrice:F2} ---");
        using var context = new OnlineLearningDbContext();

        if (newPrice < 0)
        {
            Console.WriteLine($"[Error] Price cannot be negative (${newPrice:F2}). Update rejected.");
            return false;
        }

        var course = context.Courses.Find(courseId);
        if (course == null)
        {
            Console.WriteLine($"[Error] Course with Id {courseId} was not found.");
            return false;
        }

        var oldPrice = course.Price;
        course.Price = newPrice;
        context.SaveChanges();

        Console.WriteLine($"[Success] Course '{course.Title}' price updated from ${oldPrice:F2} to ${newPrice:F2}.");
        return true;
    }

    /// <summary>
    /// Finds the enrollment by its composite key and sets the new progress.
    /// At 100 sets status to Completed. Rejects values outside 0-100.
    /// </summary>
    public static bool UpdateProgress(int studentId, int courseId, int newProgress, decimal? finalGrade = null)
    {
        Console.WriteLine($"\n--- [UPDATE] Update Progress for Student {studentId}, Course {courseId} to {newProgress}% ---");
        using var context = new OnlineLearningDbContext();

        // Business rule check: Progress must be between 0 and 100
        if (newProgress < 0 || newProgress > 100)
        {
            Console.WriteLine($"[Refusal] Progress update rejected: Value {newProgress}% is invalid. Progress must be between 0 and 100.");
            return false;
        }

        // Find enrollment by composite key
        var enrollment = context.Enrollments.Find(studentId, courseId);
        if (enrollment == null)
        {
            Console.WriteLine($"[Error] Enrollment record not found for Student Id {studentId} and Course Id {courseId}.");
            return false;
        }

        var oldProgress = enrollment.Progress;
        var oldStatus = enrollment.Status;

        enrollment.Progress = newProgress;
        if (newProgress == 100)
        {
            enrollment.Status = EnrollmentStatus.Completed;
            if (finalGrade.HasValue)
            {
                enrollment.FinalGrade = finalGrade.Value;
            }
        }

        context.SaveChanges();

        Console.WriteLine($"[Success] Enrollment updated: Progress: {oldProgress}% -> {enrollment.Progress}%, Status: {oldStatus} -> {enrollment.Status}" +
            (enrollment.FinalGrade.HasValue ? $", Final Grade: {enrollment.FinalGrade:F2}" : ""));
        return true;
    }

    /// <summary>
    /// Sets the published flag and the published date.
    /// </summary>
    public static bool PublishCourse(int courseId)
    {
        Console.WriteLine($"\n--- [UPDATE] Publish Course Id {courseId} ---");
        using var context = new OnlineLearningDbContext();

        var course = context.Courses.Find(courseId);
        if (course == null)
        {
            Console.WriteLine($"[Error] Course with Id {courseId} was not found.");
            return false;
        }

        if (course.IsPublished)
        {
            Console.WriteLine($"[Info] Course '{course.Title}' is already published on {course.PublishedDate:yyyy-MM-dd HH:mm:ss} UTC.");
            return true;
        }

        course.IsPublished = true;
        course.PublishedDate = DateTime.UtcNow;

        context.SaveChanges();

        Console.WriteLine($"[Success] Course '{course.Title}' published successfully! PublishedDate recorded: {course.PublishedDate:yyyy-MM-dd HH:mm:ss} UTC.");
        return true;
    }

    // =========================================================================
    // DELETE OPERATIONS
    // =========================================================================

    /// <summary>
    /// Deletes an enrollment using its composite key.
    /// </summary>
    public static bool Unenroll(int studentId, int courseId)
    {
        Console.WriteLine($"\n--- [DELETE] Unenroll Student {studentId} from Course {courseId} ---");
        using var context = new OnlineLearningDbContext();

        var enrollment = context.Enrollments.Find(studentId, courseId);
        if (enrollment == null)
        {
            Console.WriteLine($"[Error] Cannot unenroll: No enrollment found for Student Id {studentId} and Course Id {courseId}.");
            return false;
        }

        context.Enrollments.Remove(enrollment);
        context.SaveChanges();

        Console.WriteLine($"[Success] Successfully unenrolled Student Id {studentId} from Course Id {courseId}.");
        return true;
    }

    /// <summary>
    /// Tries to remove a category that still has sub-categories and catches the DbUpdateException.
    /// </summary>
    public static void DeleteCategoryUnsafe(int categoryId)
    {
        Console.WriteLine($"\n--- [DELETE] Unsafe Delete Category Id {categoryId} (Expected DbUpdateException demonstration) ---");
        using var context = new OnlineLearningDbContext();

        var category = context.Categories.Find(categoryId);
        if (category == null)
        {
            Console.WriteLine($"[Info] Category with Id {categoryId} does not exist.");
            return;
        }

        try
        {
            Console.WriteLine($"Attempting context.Categories.Remove('{category.Name}')...");
            context.Categories.Remove(category);
            context.SaveChanges();
            Console.WriteLine("[Unexpected] Category deleted without error.");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"[Caught Expected DbUpdateException] Delete prevented by foreign key constraint / DeleteBehavior.Restrict!");
            Console.WriteLine($"  Exception Message: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"  Inner Exception: {ex.InnerException.Message}");
            }
        }
    }

    /// <summary>
    /// Safe version that checks for sub-categories before attempting deletion.
    /// </summary>
    public static bool DeleteCategorySafe(int categoryId)
    {
        Console.WriteLine($"\n--- [DELETE] Safe Delete Category Id {categoryId} ---");
        using var context = new OnlineLearningDbContext();

        var category = context.Categories.Find(categoryId);
        if (category == null)
        {
            Console.WriteLine($"[Info] Category with Id {categoryId} was not found.");
            return false;
        }

        // Check 1: Does it have sub-categories?
        var hasSubCategories = context.Categories.Any(c => c.ParentCategoryId == categoryId);
        if (hasSubCategories)
        {
            var subCategoryCount = context.Categories.Count(c => c.ParentCategoryId == categoryId);
            Console.WriteLine($"[Safe Guard Refusal] Cannot delete Category '{category.Name}' (Id: {categoryId}) because it currently has {subCategoryCount} active sub-category/sub-categories.");
            return false;
        }

        // Check 2: Does it have courses?
        var hasCourses = context.Courses.Any(c => c.CategoryId == categoryId);
        if (hasCourses)
        {
            var courseCount = context.Courses.Count(c => c.CategoryId == categoryId);
            Console.WriteLine($"[Safe Guard Refusal] Cannot delete Category '{category.Name}' (Id: {categoryId}) because it has {courseCount} assigned course(s).");
            return false;
        }

        context.Categories.Remove(category);
        context.SaveChanges();

        Console.WriteLine($"[Success] Category '{category.Name}' (Id: {categoryId}) safely deleted.");
        return true;
    }

    /// <summary>
    /// Deletes a course that has enrollments.
    /// Prints the number of its enrollments before and after, and explains why they were removed.
    /// </summary>
    public static void DeleteCourse(int courseId)
    {
        Console.WriteLine($"\n--- [DELETE] Delete Course Id {courseId} with Cascade Verification ---");
        using var context = new OnlineLearningDbContext();

        var course = context.Courses.Find(courseId);
        if (course == null)
        {
            Console.WriteLine($"[Info] Course with Id {courseId} does not exist.");
            return;
        }

        // Count enrollments before deletion
        var enrollmentsBefore = context.Enrollments.Count(e => e.CourseId == courseId);
        Console.WriteLine($"Enrollments count BEFORE deleting course '{course.Title}': {enrollmentsBefore}");

        // Delete the course
        context.Courses.Remove(course);
        context.SaveChanges();

        // Count enrollments after deletion
        var enrollmentsAfter = context.Enrollments.Count(e => e.CourseId == courseId);
        Console.WriteLine($"Enrollments count AFTER deleting course '{course.Title}': {enrollmentsAfter}");

        // EXPLANATION COMMENT:
        // The dependent enrollments were automatically deleted from the Enrollments table
        // because the relationship between Course and Enrollment is configured with DeleteBehavior.Cascade
        // (ON DELETE CASCADE in SQL Server). When a parent Course row is deleted, SQL Server cascades
        // the deletion to all child rows referencing its primary key.
        Console.WriteLine("[Explanation] The dependent enrollments were automatically cascade-deleted by SQL Server because the foreign key relationship was configured with DeleteBehavior.Cascade.");
    }

    // =========================================================================
    // BULK OPERATIONS
    // =========================================================================

    /// <summary>
    /// In a single statement, without loading the entities, raises the price of all published courses
    /// of a given level by 10%. Prints the number of affected rows.
    /// </summary>
    public static int BulkPriceRaise(CourseLevel level, decimal percentage = 0.10m)
    {
        Console.WriteLine($"\n--- [BULK OPERATION] Bulk Price Raise of {percentage:P0} on Published Courses (Level: {level}) ---");
        using var context = new OnlineLearningDbContext();

        // EF Core ExecuteUpdate runs a single UPDATE SQL statement directly on the database
        var affectedRows = context.Courses
            .Where(c => c.IsPublished && c.Level == level)
            .ExecuteUpdate(setters => setters.SetProperty(c => c.Price, c => c.Price * (1 + percentage)));

        Console.WriteLine($"[Success] Single SQL UPDATE executed. Number of affected course rows: {affectedRows}");
        return affectedRows;
    }

    /// <summary>
    /// In a single statement, deletes all enrollments whose status is Dropped.
    /// Prints the number of affected rows.
    /// </summary>
    public static int BulkDeleteDroppedEnrollments()
    {
        Console.WriteLine($"\n--- [BULK OPERATION] Bulk Delete Dropped Enrollments ---");
        using var context = new OnlineLearningDbContext();

        // EF Core ExecuteDelete runs a single DELETE SQL statement directly on the database
        var affectedRows = context.Enrollments
            .Where(e => e.Status == EnrollmentStatus.Dropped)
            .ExecuteDelete();

        Console.WriteLine($"[Success] Single SQL DELETE executed. Number of affected enrollment rows: {affectedRows}");
        return affectedRows;
    }
}
