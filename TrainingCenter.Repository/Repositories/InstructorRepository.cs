using Microsoft.EntityFrameworkCore;
using TrainingCenter.Repository.Data;
using TrainingCenter.Repository.Entities;

namespace TrainingCenter.Repository.Repositories
{
    public class InstructorRepository
    {
        private readonly AppDbContext _context;

        public InstructorRepository(AppDbContext context)
        {
            _context = context;
        }

        // ======================================================
        // GetAll
        // ------------------------------------------------------
        // Returns all instructors from the database.
        // Includes Manager navigation property.
        // ======================================================
        public List<Instructor> GetAll()
        {
            return _context.Instructors
                           .Include(i => i.Manager)
                           .ToList();
        }

        // ======================================================
        // GetById
        // ------------------------------------------------------
        // Returns one instructor by its ID.
        // Returns null if not found.
        // ======================================================
        public Instructor? GetById(int id)
        {
            return _context.Instructors
                           .Include(i => i.Manager)
                           .Include(i => i.Courses)
                           .FirstOrDefault(i => i.InstructorId == id);
        }

        // ======================================================
        // GetAllActive
        // ------------------------------------------------------
        // Returns only active instructors.
        // ======================================================
        public List<Instructor> GetAllActive()
        {
            return _context.Instructors
                           .Where(i => i.IsActive)
                           .ToList();
        }

        // ======================================================
        // Add
        // ------------------------------------------------------
        // Inserts a new instructor into the database.
        // ======================================================
        public void Add(Instructor instructor)
        {
            _context.Instructors.Add(instructor);
        }

        // ======================================================
        // Update
        // ------------------------------------------------------
        // Updates an existing instructor in the database.
        // ======================================================
        public void Update(Instructor instructor)
        {
            _context.Instructors.Update(instructor);
        }

        // ======================================================
        // Delete
        // ------------------------------------------------------
        // Deletes an instructor by its ID.
        // Does nothing if not found.
        // ======================================================
        public void Delete(int id)
        {
            var instructor = _context.Instructors.Find(id);
            if (instructor != null)
                _context.Instructors.Remove(instructor);
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