using TrainingCenter.Repository.Data;
using TrainingCenter.Repository.Entities;
using TrainingCenter.Repository.Repositories;

namespace TrainingCenter.Service.Implementations
{
    public class StudentService
    {
        public enum enMode { AddNew = 0, Update = 1 }

        public enMode Mode = enMode.AddNew;

        public int StudentId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public DateTime RegisteredAt { get; set; }
        public string Status { get; set; } = null!;
        public string? PhoneNumber { get; set; }

        // ======================================================
        // Constructor (private)
        // ------------------------------------------------------
        // Only created via Find() or fresh instance
        // ======================================================
        private StudentService(Student student, enMode mode = enMode.AddNew)
        {
            StudentId = student.StudentId;
            FirstName = student.FirstName;
            LastName = student.LastName;
            Email = student.Email;
            DateOfBirth = student.DateOfBirth;
            RegisteredAt = student.RegisteredAt;
            Status = student.Status;
            PhoneNumber = student.PhoneNumber;
            Mode = mode;
        }

        // ======================================================
        // Constructor (public) - Add mode
        // ------------------------------------------------------
        // Used when creating a new student from the form.
        // ======================================================
        public StudentService()
        {
            Mode = enMode.AddNew;
        }

        // ======================================================
        // _AddNew (private)
        // ------------------------------------------------------
        // Inserts a new student into the database.
        // ======================================================
        private bool _AddNew()
        {
            using var context = DbContextFactory.Create();
            var repo = new StudentRepository(context);

            var student = new Student
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                DateOfBirth = this.DateOfBirth,
                RegisteredAt = DateTime.Now,
                Status = this.Status,
                PhoneNumber = this.PhoneNumber
            };

            repo.Add(student);
            repo.Save();

            this.StudentId = student.StudentId;
            return this.StudentId > 0;
        }

        // ======================================================
        // _Update (private)
        // ------------------------------------------------------
        // Updates an existing student in the database.
        // ======================================================
        private bool _Update()
        {
            using var context = DbContextFactory.Create();
            var repo = new StudentRepository(context);

            var student = repo.GetById(this.StudentId);
            if (student == null) return false;

            student.FirstName = this.FirstName;
            student.LastName = this.LastName;
            student.Email = this.Email;
            student.DateOfBirth = this.DateOfBirth;
            student.Status = this.Status;
            student.PhoneNumber = this.PhoneNumber;

            repo.Update(student);
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
        // Returns StudentService in Update mode if found.
        // Returns null if not found.
        // ======================================================
        public static StudentService? Find(int studentId)
        {
            using var context = DbContextFactory.Create();
            var repo = new StudentRepository(context);
            var student = repo.GetById(studentId);
            if (student == null) return null;
            return new StudentService(student, enMode.Update);
        }

        // ======================================================
        // GetAll (public static)
        // ------------------------------------------------------
        // Returns all students from the database.
        // ======================================================
        public static List<Student> GetAll()
        {
            using var context = DbContextFactory.Create();
            var repo = new StudentRepository(context);
            return repo.GetAll();
        }

        // ======================================================
        // Delete (public static)
        // ------------------------------------------------------
        // Deletes a student by ID.
        // ======================================================
        public static bool Delete(int studentId)
        {
            using var context = DbContextFactory.Create();
            var repo = new StudentRepository(context);
            repo.Delete(studentId);
            repo.Save();
            return true;
        }

        // ======================================================
        // IsExistByEmail (public static)
        // ======================================================
        public static bool IsExistByEmail(string email, int excludeId = 0)
        {
            using var context = DbContextFactory.Create();
            var repo = new StudentRepository(context);
            return repo.IsExistByEmail(email, excludeId);
        }

        // ======================================================
        // IsExistByPhone (public static)
        // ======================================================
        public static bool IsExistByPhone(string phone, int excludeId = 0)
        {
            using var context = DbContextFactory.Create();
            var repo = new StudentRepository(context);
            return repo.IsExistByPhone(phone, excludeId);
        }
    }
}