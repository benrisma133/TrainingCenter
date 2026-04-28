using Microsoft.EntityFrameworkCore;
using TrainingCenter.Repository.Data;
using TrainingCenter.Repository.Entities;

namespace TrainingCenter.Repository.Repositories
{
    public class EnrollmentRepository
    {
        private readonly AppDbContext _context;

        public EnrollmentRepository(AppDbContext context)
        {
            _context = context;
        }

        // ======================================================
        // GetAll
        // ------------------------------------------------------
        // Returns all enrollments from the database.
        // Includes Student and Course navigation properties.
        // ======================================================
        public List<Enrollment> GetAll()
        {
            return _context.Enrollments
                           .Include(e => e.Student)
                           .Include(e => e.Course)
                           .ToList();
        }

        // ======================================================
        // GetById
        // ------------------------------------------------------
        // Returns one enrollment by its ID.
        // Returns null if not found.
        // ======================================================
        public Enrollment? GetById(int id)
        {
            return _context.Enrollments
                           .Include(e => e.Student)
                           .Include(e => e.Course)
                           .FirstOrDefault(e => e.EnrollmentId == id);
        }

        // ======================================================
        // GetByStudentId
        // ------------------------------------------------------
        // Returns all enrollments for a specific student.
        // ======================================================
        public List<Enrollment> GetByStudentId(int studentId)
        {
            return _context.Enrollments
                           .Include(e => e.Course)
                           .Where(e => e.StudentId == studentId)
                           .ToList();
        }

        // ======================================================
        // GetByCourseId
        // ------------------------------------------------------
        // Returns all enrollments for a specific course.
        // ======================================================
        public List<Enrollment> GetByCourseId(int courseId)
        {
            return _context.Enrollments
                           .Include(e => e.Student)
                           .Where(e => e.CourseId == courseId)
                           .ToList();
        }

        // ======================================================
        // Add
        // ------------------------------------------------------
        // Inserts a new enrollment into the database.
        // ======================================================
        public void Add(Enrollment enrollment)
        {
            _context.Enrollments.Add(enrollment);
        }

        // ======================================================
        // Update
        // ------------------------------------------------------
        // Updates an existing enrollment in the database.
        // ======================================================
        public void Update(Enrollment enrollment)
        {
            _context.Enrollments.Update(enrollment);
        }

        // ======================================================
        // Delete
        // ------------------------------------------------------
        // Deletes an enrollment by its ID.
        // Does nothing if not found.
        // ======================================================
        public void Delete(int id)
        {
            var enrollment = _context.Enrollments.Find(id);
            if (enrollment != null)
                _context.Enrollments.Remove(enrollment);
        }

        // ======================================================
        // Save
        // ------------------------------------------------------
        // Commits all pending changes to the database.
        // Always call after Add / Update / Delete.
        // ======================================================
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}