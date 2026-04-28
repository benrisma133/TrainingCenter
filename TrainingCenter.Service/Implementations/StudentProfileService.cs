using TrainingCenter.Repository.Data;
using TrainingCenter.Repository.Entities;
using TrainingCenter.Repository.Repositories;

namespace TrainingCenter.Service.Implementations
{
    public class StudentProfileService
    {
        public enum enMode { AddNew = 0, Update = 1 }

        public enMode Mode = enMode.AddNew;

        public int StudentId { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Bio { get; set; }
        public string? LinkedInUrl { get; set; }

        // ======================================================
        // Constructor (private)
        // ------------------------------------------------------
        // Only created via Find() or fresh instance
        // ======================================================
        private StudentProfileService(StudentProfile profile, enMode mode = enMode.AddNew)
        {
            StudentId = profile.StudentId;
            Address = profile.Address;
            City = profile.City;
            Country = profile.Country;
            Bio = profile.Bio;
            LinkedInUrl = profile.LinkedInUrl;
            Mode = mode;
        }

        // ======================================================
        // _AddNew (private)
        // ------------------------------------------------------
        // Creates a new profile for a student.
        // ======================================================
        private bool _AddNew()
        {
            using var context = DbContextFactory.Create();
            var repo = new StudentProfileRepository(context);

            var profile = new StudentProfile
            {
                StudentId = this.StudentId,
                Address = this.Address,
                City = this.City,
                Country = this.Country,
                Bio = this.Bio,
                LinkedInUrl = this.LinkedInUrl
            };

            repo.Add(profile);
            repo.Save();

            return true;
        }

        // ======================================================
        // _Update (private)
        // ------------------------------------------------------
        // Updates an existing student profile.
        // ======================================================
        private bool _Update()
        {
            using var context = DbContextFactory.Create();
            var repo = new StudentProfileRepository(context);

            var profile = repo.GetByStudentId(this.StudentId);
            if (profile == null) return false;

            profile.Address = this.Address;
            profile.City = this.City;
            profile.Country = this.Country;
            profile.Bio = this.Bio;
            profile.LinkedInUrl = this.LinkedInUrl;

            repo.Update(profile);
            repo.Save();
            return true;
        }

        // ======================================================
        // Save (public)
        // ------------------------------------------------------
        // Decides AddNew or Update based on Mode.
        // Switches to Update after successful AddNew.
        // ======================================================
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNew())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _Update();

                default:
                    return false;
            }
        }

        // ======================================================
        // Find (public static)
        // ------------------------------------------------------
        // Returns StudentProfileService in Update mode if found.
        // Returns null if no profile exists for this student.
        // ======================================================
        public static StudentProfileService? Find(int studentId)
        {
            using var context = DbContextFactory.Create();
            var repo = new StudentProfileRepository(context);
            var profile = repo.GetByStudentId(studentId);
            if (profile == null) return null;
            return new StudentProfileService(profile, enMode.Update);
        }

        // ======================================================
        // Delete (public static)
        // ------------------------------------------------------
        // Deletes a student profile by StudentId.
        // ======================================================
        public static bool Delete(int studentId)
        {
            using var context = DbContextFactory.Create();
            var repo = new StudentProfileRepository(context);
            repo.Delete(studentId);
            repo.Save();
            return true;
        }
    }
}