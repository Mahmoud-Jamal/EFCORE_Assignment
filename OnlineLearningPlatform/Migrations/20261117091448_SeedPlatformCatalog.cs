using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnlineLearningPlatform.Migrations
{
    /// <inheritdoc />
    public partial class SeedPlatformCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name", "ParentCategoryId" },
                values: new object[,]
                {
                    { 1, "Software engineering, algorithms, and application development.", "Software Development", null },
                    { 2, "Data analysis, machine learning, and artificial intelligence.", "Data Science & AI", null }
                });

            migrationBuilder.InsertData(
                table: "Instructors",
                columns: new[] { "Id", "Bio", "Email", "FullName", "JoinedDate" },
                values: new object[,]
                {
                    { 1, "Principal Software Architect with 15+ years experience in .NET and cloud distributed systems.", "ahmed.ali@platform.local", "Ahmed Ali", new DateOnly(2021, 3, 15) },
                    { 2, "Lead Data Engineer and SQL Server MVP specializing in relational performance and data warehousing.", "mona.hassan@platform.local", "Mona Hassan", new DateOnly(2022, 6, 1) },
                    { 3, null, "tarek.mansour@platform.local", "Tarek Mansour", new DateOnly(2023, 1, 10) }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name", "ParentCategoryId" },
                values: new object[,]
                {
                    { 3, "Server-side architectures, APIs, and business logic.", "Backend Engineering", 1 },
                    { 4, "Client-side interfaces, modern web frameworks, and UX.", "Frontend Engineering", 1 },
                    { 5, "Relational modeling, query optimization, and storage.", "Database Systems", 2 }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CategoryId", "Description", "InstructorId", "IsPublished", "Level", "Price", "PublishedDate", "Title" },
                values: new object[] { 1, 1, "Comprehensive introduction to C# syntax, OOP, and modern language features.", 1, true, "Beginner", 49.99m, new DateTime(2024, 1, 15, 10, 0, 0, 0, DateTimeKind.Utc), "C# Fundamentals for Beginners" });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CategoryId", "Description", "InstructorId", "Level", "Price", "PublishedDate", "Title" },
                values: new object[] { 7, 2, "Integrating LLMs, semantic kernel, and vector embeddings in enterprise apps.", 3, "Advanced", 129.99m, null, "Generative AI Engineering with Python & C#" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name", "ParentCategoryId" },
                values: new object[,]
                {
                    { 6, "Enterprise web APIs, microservices, and security with .NET.", "ASP.NET Core", 3 },
                    { 7, "ORM mastery, migrations, and high-performance querying.", "EF Core & SQL", 5 }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CategoryId", "Description", "InstructorId", "Level", "Price", "PublishedDate", "Title" },
                values: new object[] { 3, 4, "Building scalable single page applications with Angular and TypeScript.", 3, "Intermediate", 69.99m, null, "Modern Angular & Micro-Frontends" });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CategoryId", "Description", "InstructorId", "IsPublished", "Level", "Price", "PublishedDate", "Title" },
                values: new object[,]
                {
                    { 4, 5, "Normalization, indexing, transaction management, and stored procedures.", 2, true, "Beginner", 59.99m, new DateTime(2024, 3, 10, 9, 30, 0, 0, DateTimeKind.Utc), "Relational Database Design with SQL Server" },
                    { 6, 3, "Event-driven design, message brokers, containerization, and resilience patterns.", 1, true, "Advanced", 119.99m, new DateTime(2024, 5, 1, 11, 0, 0, 0, DateTimeKind.Utc), "Cloud-Native Microservices Architecture" },
                    { 2, 6, "Production-ready RESTful APIs, dependency injection, and authentication.", 1, true, "Intermediate", 89.99m, new DateTime(2024, 2, 20, 14, 0, 0, 0, DateTimeKind.Utc), "Mastering ASP.NET Core Web APIs" },
                    { 5, 7, "Advanced query tuning, compiled models, concurrency handling, and bulk operations.", 2, true, "Advanced", 99.99m, new DateTime(2024, 4, 5, 16, 0, 0, 0, DateTimeKind.Utc), "High-Performance EF Core 10" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
