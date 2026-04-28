using Microsoft.EntityFrameworkCore;
using TrainingCenter.Repository.Data;
using TrainingCenter.Repository.Entities;

namespace TrainingCenter.Repository.Repositories
{
    public class StudentProfileRepository
    {
        private readonly AppDbContext _context;

        public StudentProfileRepository(AppDbContext context)
        {
            _context = context;
        }

        // ======================================================
        // GetByStudentId
        // ------------------------------------------------------
        // Returns the profile of a specific student.
        // Returns null if no profile exists yet.
        // ======================================================
        public StudentProfile? GetByStudentId(int studentId)
        {
            return _context.StudentProfiles
                           .Include(sp => sp.Student)
                           .FirstOrDefault(sp => sp.StudentId == studentId);
        }

        // ======================================================
        // Add
        // ------------------------------------------------------
        // Creates a new profile for a student.
        // ======================================================
        public void Add(StudentProfile profile)
        {
            _context.StudentProfiles.Add(profile);
        }

        // ======================================================
        // Update
        // ------------------------------------------------------
        // Updates an existing student profile.
        // ======================================================
        public void Update(StudentProfile profile)
        {
            _context.StudentProfiles.Update(profile);
        }

        // ======================================================
        // Delete
        // ------------------------------------------------------
        // Deletes a student profile by StudentId.
        // Does nothing if not found.
        // ======================================================
        public void Delete(int studentId)
        {
            var profile = _context.StudentProfiles.Find(studentId);
            if (profile != null)
                _context.StudentProfiles.Remove(profile);
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