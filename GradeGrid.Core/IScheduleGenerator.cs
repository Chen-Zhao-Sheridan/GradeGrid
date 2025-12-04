using GradeGrid.Core.DTOs;
using GradeGrid.Core.Models;

namespace GradeGrid.Core
{
    public interface IScheduleGenerator
    {
        public List<GeneratedScheduleDto> GenerateValidSchedules(List<Course> courses);
    }
}