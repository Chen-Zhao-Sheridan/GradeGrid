using GradeGrid.Core.Models;

namespace GradeGrid.Infrastructure
{
    public class CourseRepository : ICourseRepository
    {
        private GradeGridDbContext _context;
        public CourseRepository(GradeGridDbContext ctx)
        {
            _context = ctx;
        }

        public void Add(Course request)
        {
            _context.Courses.Add(request);
            _context.SaveChanges();
        }

        public void Delete(int courseId)
        {
            var courseToRemove = FindById(courseId);
            if (courseToRemove != null)
            {
                _context.Courses.Remove(courseToRemove);
                _context.SaveChanges();
            }
        }

        public Course? FindById(int Id)
        {
            return _context.Courses.Find(Id);
        }

        public List<Course> GetAll()
        {
            return _context.Courses.ToList();
        }
    }
}
