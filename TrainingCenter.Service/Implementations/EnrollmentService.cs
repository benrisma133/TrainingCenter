using TrainingCenter.Repository.Data;
using TrainingCenter.Repository.Entities;
using TrainingCenter.Repository.Repositories;

namespace TrainingCenter.Service.Implementations
{
    public class EnrollmentService
    {
        public enum enMode { AddNew = 0, Update = 1 }

        public enMode Mode = enMode.AddNew;

        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public decimal ProgressPercent { get; set; }
        public decimal? FinalGrade { get; set; }
        public string Status { get; set; } = null!;

        // ======================================================
        // Constructor (private)
        // ------------------------------------------------------
        // Only created via Find() or fresh instance
        // ======================================================
        private EnrollmentService(Enrollment enrollment, enMode mode = enMode.AddNew)
        {
            EnrollmentId = enrollment.EnrollmentId;
            StudentId = enrollment.StudentId;
            CourseId = enrollment.CourseId;
            EnrollmentDate = enrollment.EnrollmentDate;
            CompletionDate = enrollment.CompletionDate;
            ProgressPercent = enrollment.ProgressPercent;
            FinalGrade = enrollment.FinalGrade;
            Status = enrollment.Status;
            Mode = mode;
        }

        // ======================================================
        // _AddNew (private)
        // ------------------------------------------------------
        // Inserts a new enrollment into the database.
        // ======================================================
        private bool _AddNew()
        {
            using var context = DbContextFactory.Create();
            var repo = new EnrollmentRepository(context);

            var enrollment = new Enrollment
            {
                StudentId = this.StudentId,
                CourseId = this.CourseId,
                EnrollmentDate = DateTime.Now,
                CompletionDate = this.CompletionDate,
                ProgressPercent = this.ProgressPercent,
                FinalGrade = this.FinalGrade,
                Status = this.Status
            };

            repo.Add(enrollment);
            repo.Save();

            this.EnrollmentId = enrollment.EnrollmentId;
            return this.EnrollmentId > 0;
        }

        // ======================================================
        // _Update (private)
        // ------------------------------------------------------
        // Updates an existing enrollment in the database.
        // ======================================================
        private bool _Update()
        {
            using var context = DbContextFactory.Create();
            var repo = new EnrollmentRepository(context);

            var enrollment = repo.GetById(this.EnrollmentId);
            if (enrollment == null) return false;

            enrollment.CompletionDate = this.CompletionDate;
            enrollment.ProgressPercent = this.ProgressPercent;
            enrollment.FinalGrade = this.FinalGrade;
            enrollment.Status = this.Status;

            repo.Update(enrollment);
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
        // Returns EnrollmentService in Update mode if found.
        // Returns null if not found.
        // ======================================================
        public static EnrollmentService? Find(int enrollmentId)
        {
            using var context = DbContextFactory.Create();
            var repo = new EnrollmentRepository(context);
            var enrollment = repo.GetById(enrollmentId);
            if (enrollment == null) return null;
            return new EnrollmentService(enrollment, enMode.Update);
        }

        // ======================================================
        // GetAll (public static)
        // ------------------------------------------------------
        // Returns all enrollments from the database.
        // ======================================================
        public static List<Enrollment> GetAll()
        {
            using var context = DbContextFactory.Create();
            var repo = new EnrollmentRepository(context);
            return repo.GetAll();
        }

        // ======================================================
        // GetByStudentId (public static)
        // ------------------------------------------------------
        // Returns all enrollments for a specific student.
        // ======================================================
        public static List<Enrollment> GetByStudentId(int studentId)
        {
            using var context = DbContextFactory.Create();
            var repo = new EnrollmentRepository(context);
            return repo.GetByStudentId(studentId);
        }

        // ======================================================
        // GetByCourseId (public static)
        // ------------------------------------------------------
        // Returns all enrollments for a specific course.
        // ======================================================
        public static List<Enrollment> GetByCourseId(int courseId)
        {
            using var context = DbContextFactory.Create();
            var repo = new EnrollmentRepository(context);
            return repo.GetByCourseId(courseId);
        }

        // ======================================================
        // Delete (public static)
        // ------------------------------------------------------
        // Deletes an enrollment by ID.
        // ======================================================
        public static bool Delete(int enrollmentId)
        {
            using var context = DbContextFactory.Create();
            var repo = new EnrollmentRepository(context);
            repo.Delete(enrollmentId);
            repo.Save();
            return true;
        }
    }
}