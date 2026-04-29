using Microsoft.EntityFrameworkCore;
using TrainingCenter.Repository.Data;
using TrainingCenter.Repository.Entities;

namespace TrainingCenter.Repository.Repositories
{
    public class CourseRepository
    {
        private readonly AppDbContext _context;

        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }

        // ======================================================
        // GetAll
        // ------------------------------------------------------
        // Returns all courses from the database.
        // Includes Instructor navigation property.
        // ======================================================
        public List<Course> GetAll()
        {
            return _context.Courses
                           .Include(c => c.Instructor)
                           .ToList();
        }

        // ======================================================
        // GetById
        // ------------------------------------------------------
        // Returns one course by its ID.
        // Returns null if not found.
        // ======================================================
        public Course? GetById(int id)
        {
            return _context.Courses
                           .Include(c => c.Instructor)
                           .FirstOrDefault(c => c.CourseId == id);
        }

        // ======================================================
        // Add
        // ------------------------------------------------------
        // Inserts a new course into the database.
        // ======================================================
        public void Add(Course course)
        {
            _context.Courses.Add(course);
        }

        // ======================================================
        // Update
        // ------------------------------------------------------
        // Updates an existing course in the database.
        // ======================================================
        public void Update(Course course)
        {
            _context.Courses.Update(course);
        }

        // ======================================================
        // Delete
        // ------------------------------------------------------
        // Deletes a course by its ID.
        // Does nothing if not found.
        // ======================================================
        public void Delete(int id)
        {
            var course = _context.Courses.Find(id);
            if (course != null)
                _context.Courses.Remove(course);
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

        // ======================================================
        // IsExistByEmail
        // ------------------------------------------------------
        // Returns true if another student has the same email.
        // Excludes the current student when updating.
        // ======================================================
        public bool IsExistByCode(string code, int excludeId = 0)
        {
            return _context.Courses
                           .Any(c => c.Code == code && c.CourseId != excludeId);
        }
    }
}