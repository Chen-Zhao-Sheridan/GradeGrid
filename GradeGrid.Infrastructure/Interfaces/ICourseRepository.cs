using GradeGrid.Core.Enums;
using GradeGrid.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeGrid.Infrastructure.Interfaces
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAll();
        Task<Course?> FindById(int id);
        Task Add(Course course);
        Task Update(Course course);
        Task Delete(int id);
        Task<List<Course>> GetCoursesBySemester(Term term, int year);
        Task<List<Course>> GetCoursesWithSections(List<int> courseIds);
        Task<bool> Exists(string courseCode, Term term, int year); // added to enforce this better then just crashing
    }
}

