using System;
using System.Collections.Generic;

namespace TrainingCenter.Repository.Entities
{
    // ==========================================================
    // Course Entity Class
    // ----------------------------------------------------------
    // This class represents the Courses table in the database.
    //
    // Each object created from this class represents one row
    // inside the Courses table.
    //
    // Example:
    // CourseId = 1
    // Title    = "C# Fundamentals"
    // Price    = 99.99
    // ==========================================================
    public partial class Course
    {

        // ------------------------------------------------------
        // Primary Key
        // ------------------------------------------------------
        // Unique identity for each course.
        // Usually auto-incremented by SQL Server.
        //
        // Example:
        // 1, 2, 3, 4...
        // ------------------------------------------------------
        public int CourseId { get; set; }

        // ------------------------------------------------------
        // Course Title
        // ------------------------------------------------------
        // Name of the course shown to students.
        //
        // Examples:
        // "C# Fundamentals"
        // "ASP.NET Core Web API"
        // ------------------------------------------------------
        public string Title { get; set; } = null!;

        // ------------------------------------------------------
        // Course Code
        // ------------------------------------------------------
        // Unique short code used internally.
        //
        // Examples:
        // CSH101
        // API205
        // SQL300
        // ------------------------------------------------------
        public string Code { get; set; } = null!;

        // ------------------------------------------------------
        // Description
        // ------------------------------------------------------
        // Optional summary of the course.
        //
        // Nullable (?) means it can contain null.
        // Some courses may not have description yet.
        // ------------------------------------------------------
        public string? Description { get; set; }

        // ------------------------------------------------------
        // Price
        // ------------------------------------------------------
        // Cost of the course.
        //
        // Decimal is used for money values.
        //
        // Examples:
        // 49.99
        // 100.00
        // ------------------------------------------------------
        public decimal Price { get; set; }

        // ------------------------------------------------------
        // Level
        // ------------------------------------------------------
        // Difficulty level of the course.
        //
        // Examples:
        // Beginner
        // Intermediate
        // Advanced
        // ------------------------------------------------------
        public string Level { get; set; } = null!;

        // ------------------------------------------------------
        // DurationHours
        // ------------------------------------------------------
        // Total number of course hours.
        //
        // Example:
        // 20 = twenty hours
        // ------------------------------------------------------
        public int DurationHours { get; set; }

        // ------------------------------------------------------
        // CreatedAt
        // ------------------------------------------------------
        // Date when the course record was created.
        //
        // Example:
        // 2026-04-20
        // ------------------------------------------------------
        public DateTime CreatedAt { get; set; }

        // ------------------------------------------------------
        // PublishedAt
        // ------------------------------------------------------
        // Date when course became publicly available.
        //
        // Nullable (?) means:
        // If still draft → null
        // ------------------------------------------------------
        public DateTime? PublishedAt { get; set; }

        // ------------------------------------------------------
        // Status
        // ------------------------------------------------------
        // Current course state.
        //
        // Examples:
        // Draft
        // Active
        // Archived
        // Closed
        // ------------------------------------------------------
        public string Status { get; set; } = null!;

        // ------------------------------------------------------
        // Foreign Key
        // ------------------------------------------------------
        // Links this course to its instructor.
        //
        // Example:
        // InstructorId = 5
        // means instructor with ID 5 teaches this course.
        // ------------------------------------------------------
        public int InstructorId { get; set; }

        // ======================================================
        // Navigation Properties
        // ------------------------------------------------------
        // These properties represent relationships between tables.
        // ======================================================

        // ------------------------------------------------------
        // Enrollments Relationship
        // ------------------------------------------------------
        // One course can have many enrollments.
        //
        // Example:
        // Course "C#" may have 100 students enrolled.
        // ------------------------------------------------------
        public virtual ICollection<Enrollment> Enrollments { get; set; }
            = new List<Enrollment>();

        // ------------------------------------------------------
        // Instructor Relationship
        // ------------------------------------------------------
        // Each course belongs to one instructor.
        //
        // Example:
        // Course.Instructor.FullName
        // ------------------------------------------------------
        public virtual Instructor Instructor { get; set; } = null!;


    }
}
