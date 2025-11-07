using GradeGrid.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeGrid.Infrastructure
{
    public interface ICourseRepository
    {
        List<Course> GetAll();
        Course? FindById(int Id);
        void Add(Course request);
        void Delete(int requestId);
    }
}

