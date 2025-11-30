using GradeGrid.Core.Enums;
using GradeGrid.Core.Models;
using GradeGrid.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GradeGrid.Infrastructure
{
    public class CourseRepository : ICourseRepository
    {
        private GradeGridDbContext _context;
        public CourseRepository(GradeGridDbContext ctx)
        {
            _context = ctx;
        }

        public async Task Add(Course course)
        {
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int courseId)
        {
            var courseToRemove = await FindById(courseId);
            if (courseToRemove != null)
            {
                _context.Courses.Remove(courseToRemove);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Course?> FindById(int id)
        {
            // Remember to actually include the nav properties
            return await _context.Courses
                .Include(c => c.Sections)
                .ThenInclude(s => s.TimeSlots)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Course>> GetAll()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<List<Course>> GetCoursesBySemester(Term term, int year)
        {
            return await _context.Courses
                .Where(c => c.Term == term && c.Year == year)
                .Include(c => c.Sections)
                .ToListAsync();
        }

        public async Task<List<Course>> GetCoursesWithSections(List<int> courseIds)
        {
            return await _context.Courses
                .Where(c => courseIds.Contains(c.Id))
                .Include(c => c.Sections)
                .ThenInclude(s => s.TimeSlots)
                .ToListAsync();
        }
    }
}
