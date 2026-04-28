using Microsoft.EntityFrameworkCore;
using TrainingCenter.Repository.Data;
using TrainingCenter.Repository.Entities;

namespace TrainingCenter.Repository.Repositories
{
    public class StudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        // ======================================================
        // GetAll
        // ------------------------------------------------------
        // Returns all students from the database.
        // Includes StudentProfile navigation property.
        // ======================================================
        public List<Student> GetAll()
        {
            return _context.Students
                           .Include(s => s.StudentProfile)
                           .ToList();
        }

        // ======================================================
        // GetById
        // ------------------------------------------------------
        // Returns one student by its ID.
        // Returns null if not found.
        // ======================================================
        public Student? GetById(int id)
        {
            return _context.Students
                           .Include(s => s.StudentProfile)
                           .FirstOrDefault(s => s.StudentId == id);
        }

        // ======================================================
        // Add
        // ------------------------------------------------------
        // Inserts a new student into the database.
        // ======================================================
        public void Add(Student student)
        {
            _context.Students.Add(student);
        }

        // ======================================================
        // Update
        // ------------------------------------------------------
        // Updates an existing student in the database.
        // ======================================================
        public void Update(Student student)
        {
            _context.Students.Update(student);
        }

        // ======================================================
        // Delete
        // ------------------------------------------------------
        // Deletes a student by its ID.
        // Does nothing if not found.
        // ======================================================
        public void Delete(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
                _context.Students.Remove(student);
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