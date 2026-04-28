using TrainingCenter.Repository.Data;
using TrainingCenter.Repository.Entities;
using TrainingCenter.Repository.Repositories;

namespace TrainingCenter.Service.Implementations
{
    public class InstructorService
    {
        public enum enMode { AddNew = 0, Update = 1 }

        public enMode Mode = enMode.AddNew;

        public int InstructorId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateOnly HireDate { get; set; }
        public decimal Salary { get; set; }
        public int? ManagerId { get; set; }
        public bool IsActive { get; set; }

        // ======================================================
        // Constructor (private)
        // ------------------------------------------------------
        // Only created via Find() or fresh instance
        // ======================================================
        private InstructorService(Instructor instructor, enMode mode = enMode.AddNew)
        {
            InstructorId = instructor.InstructorId;
            FirstName = instructor.FirstName;
            LastName = instructor.LastName;
            Email = instructor.Email;
            HireDate = instructor.HireDate;
            Salary = instructor.Salary;
            ManagerId = instructor.ManagerId;
            IsActive = instructor.IsActive;
            Mode = mode;
        }

        // ======================================================
        // _AddNew (private)
        // ------------------------------------------------------
        // Inserts a new instructor into the database.
        // ======================================================
        private bool _AddNew()
        {
            using var context = DbContextFactory.Create();
            var repo = new InstructorRepository(context);

            var instructor = new Instructor
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                HireDate = this.HireDate,
                Salary = this.Salary,
                ManagerId = this.ManagerId,
                IsActive = this.IsActive
            };

            repo.Add(instructor);
            repo.Save();

            this.InstructorId = instructor.InstructorId;
            return this.InstructorId > 0;
        }

        // ======================================================
        // _Update (private)
        // ------------------------------------------------------
        // Updates an existing instructor in the database.
        // ======================================================
        private bool _Update()
        {
            using var context = DbContextFactory.Create();
            var repo = new InstructorRepository(context);

            var instructor = repo.GetById(this.InstructorId);
            if (instructor == null) return false;

            instructor.FirstName = this.FirstName;
            instructor.LastName = this.LastName;
            instructor.Email = this.Email;
            instructor.HireDate = this.HireDate;
            instructor.Salary = this.Salary;
            instructor.ManagerId = this.ManagerId;
            instructor.IsActive = this.IsActive;

            repo.Update(instructor);
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
        // Returns InstructorService in Update mode if found.
        // Returns null if not found.
        // ======================================================
        public static InstructorService? Find(int instructorId)
        {
            using var context = DbContextFactory.Create();
            var repo = new InstructorRepository(context);
            var instructor = repo.GetById(instructorId);
            if (instructor == null) return null;
            return new InstructorService(instructor, enMode.Update);
        }

        // ======================================================
        // GetAll (public static)
        // ------------------------------------------------------
        // Returns all instructors from the database.
        // ======================================================
        public static List<Instructor> GetAll()
        {
            using var context = DbContextFactory.Create();
            var repo = new InstructorRepository(context);
            return repo.GetAll();
        }

        // ======================================================
        // GetAllActive (public static)
        // ------------------------------------------------------
        // Returns only active instructors.
        // ======================================================
        public static List<Instructor> GetAllActive()
        {
            using var context = DbContextFactory.Create();
            var repo = new InstructorRepository(context);
            return repo.GetAllActive();
        }

        // ======================================================
        // Delete (public static)
        // ------------------------------------------------------
        // Deletes an instructor by ID.
        // ======================================================
        public static bool Delete(int instructorId)
        {
            using var context = DbContextFactory.Create();
            var repo = new InstructorRepository(context);
            repo.Delete(instructorId);
            repo.Save();
            return true;
        }
    }
}