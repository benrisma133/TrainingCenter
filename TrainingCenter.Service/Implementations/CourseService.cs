using TrainingCenter.Repository.Data;
using TrainingCenter.Repository.Entities;
using TrainingCenter.Repository.Repositories;

namespace TrainingCenter.Service.Implementations
{
    public class CourseService
    {
        public enum enMode { AddNew = 0, Update = 1 }

        public enMode Mode = enMode.AddNew;

        public int CourseId { get; set; }
        public string Title { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string Level { get; set; } = null!;
        public int DurationHours { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string Status { get; set; } = null!;
        public int InstructorId { get; set; }

        // ======================================================
        // Constructor
        // ------------------------------------------------------
        // Private — only created via Find() or new instance
        // ======================================================
        private CourseService(Course course, enMode mode = enMode.AddNew)
        {
            CourseId = course.CourseId;
            Title = course.Title;
            Code = course.Code;
            Description = course.Description;
            Price = course.Price;
            Level = course.Level;
            DurationHours = course.DurationHours;
            CreatedAt = course.CreatedAt;
            PublishedAt = course.PublishedAt;
            Status = course.Status;
            InstructorId = course.InstructorId;
            Mode = mode;
        }

        // ======================================================
        // _AddNew (private)
        // ------------------------------------------------------
        // Inserts a new course into the database.
        // ======================================================
        private bool _AddNew()
        {
            using var context = DbContextFactory.Create();
            var repo = new CourseRepository(context);

            var course = new Course
            {
                Title = this.Title,
                Code = this.Code,
                Description = this.Description,
                Price = this.Price,
                Level = this.Level,
                DurationHours = this.DurationHours,
                CreatedAt = DateTime.Now,
                PublishedAt = this.PublishedAt,
                Status = this.Status,
                InstructorId = this.InstructorId
            };

            repo.Add(course);
            repo.Save();

            this.CourseId = course.CourseId;
            return this.CourseId > 0;
        }

        // ======================================================
        // _Update (private)
        // ------------------------------------------------------
        // Updates an existing course in the database.
        // ======================================================
        private bool _Update()
        {
            using var context = DbContextFactory.Create();
            var repo = new CourseRepository(context);

            var course = repo.GetById(this.CourseId);
            if (course == null) return false;

            course.Title = this.Title;
            course.Code = this.Code;
            course.Description = this.Description;
            course.Price = this.Price;
            course.Level = this.Level;
            course.DurationHours = this.DurationHours;
            course.PublishedAt = this.PublishedAt;
            course.Status = this.Status;
            course.InstructorId = this.InstructorId;

            repo.Update(course);
            repo.Save();
            return true;
        }

        // ======================================================
        // Save (public)
        // ------------------------------------------------------
        // Decides AddNew or Update based on Mode.
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
        // Returns CourseService in Update mode if found.
        // Returns null if not found.
        // ======================================================
        public static CourseService? Find(int courseId)
        {
            using var context = DbContextFactory.Create();
            var repo = new CourseRepository(context);
            var course = repo.GetById(courseId);
            if (course == null) return null;
            return new CourseService(course, enMode.Update);
        }

        // ======================================================
        // GetAll (public static)
        // ------------------------------------------------------
        // Returns all courses from the database.
        // ======================================================
        public static List<Course> GetAll()
        {
            using var context = DbContextFactory.Create();
            var repo = new CourseRepository(context);
            return repo.GetAll();
        }

        // ======================================================
        // Delete (public static)
        // ------------------------------------------------------
        // Deletes a course by ID.
        // ======================================================
        public static bool Delete(int courseId)
        {
            using var context = DbContextFactory.Create();
            var repo = new CourseRepository(context);
            repo.Delete(courseId);
            repo.Save();
            return true;
        }
    }
}