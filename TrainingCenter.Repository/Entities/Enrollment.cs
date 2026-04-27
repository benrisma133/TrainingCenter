using System;
using System.Collections.Generic;
using TrainingCenter.Repository.Entities;

namespace TrainingCenter.Repository.Entities
{
    // ==========================================================
    // Enrollment Entity Class
    // ----------------------------------------------------------
    // This class represents the Enrollments table
    // in the database.
    //
    // Each object created from this class represents
    // one student registration inside one course.
    //
    // Example:
    // Student Ahmed enrolled in C# Course
    // ==========================================================
    public partial class Enrollment
    {

        // ------------------------------------------------------
        // Primary Key
        // ------------------------------------------------------
        // Unique ID for each enrollment record.
        //
        // Example:
        // 1, 2, 3, 4...
        // ------------------------------------------------------
        public int EnrollmentId { get; set; }

        // ------------------------------------------------------
        // Foreign Key → Student
        // ------------------------------------------------------
        // Indicates which student owns this enrollment.
        //
        // Example:
        // StudentId = 7
        // means student with ID 7 enrolled.
        // ------------------------------------------------------
        public int StudentId { get; set; }

        // ------------------------------------------------------
        // Foreign Key → Course
        // ------------------------------------------------------
        // Indicates which course the student joined.
        //
        // Example:
        // CourseId = 3
        // means course with ID 3.
        // ------------------------------------------------------
        public int CourseId { get; set; }

        // ------------------------------------------------------
        // EnrollmentDate
        // ------------------------------------------------------
        // Date when the student enrolled in the course.
        //
        // Example:
        // 2026-04-20
        // ------------------------------------------------------
        public DateTime EnrollmentDate { get; set; }

        // ------------------------------------------------------
        // CompletionDate
        // ------------------------------------------------------
        // Date when student finished the course.
        //
        // Nullable (?) means:
        // If student is still studying,
        // then this value can be null.
        // ------------------------------------------------------
        public DateTime? CompletionDate { get; set; }

        // ------------------------------------------------------
        // ProgressPercent
        // ------------------------------------------------------
        // Shows student progress in the course.
        //
        // Examples:
        // 0     = not started
        // 35.50 = in progress
        // 100   = completed
        // ------------------------------------------------------
        public decimal ProgressPercent { get; set; }

        // ------------------------------------------------------
        // FinalGrade
        // ------------------------------------------------------
        // Final score after finishing course.
        //
        // Nullable (?) means:
        // If course not completed yet,
        // then grade may still be null.
        //
        // Examples:
        // 95.50
        // 88.00
        // ------------------------------------------------------
        public decimal? FinalGrade { get; set; }

        // ------------------------------------------------------
        // Status
        // ------------------------------------------------------
        // Current enrollment state.
        //
        // Examples:
        // Active
        // Completed
        // Cancelled
        // Suspended
        // ------------------------------------------------------
        public string Status { get; set; } = null!;

        // ======================================================
        // Navigation Properties
        // ------------------------------------------------------
        // These properties represent relationships.
        // ======================================================

        // ------------------------------------------------------
        // Course Relationship
        // ------------------------------------------------------
        // This enrollment belongs to one course.
        //
        // Example:
        // enrollment.Course.Title
        // ------------------------------------------------------
        public virtual Course Course { get; set; } = null!;

        // ------------------------------------------------------
        // Student Relationship
        // ------------------------------------------------------
        // This enrollment belongs to one student.
        //
        // Example:
        // enrollment.Student.FirstName
        // ------------------------------------------------------
        public virtual Student Student { get; set; } = null!;

    }
}
