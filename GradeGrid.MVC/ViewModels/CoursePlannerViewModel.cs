using GradeGrid.Core.Enums;
using GradeGrid.Core.Models;

namespace GradeGrid.MVC.ViewModels
{
    public class CoursePlannerViewModel
    {
        public int Year { get; set; }
        public Term Term { get; set; }
        public List<CourseSummary> AvailableCourses { get; set; } = new();
        public List<int> SelectedCourseIds { get; set; } = new();
        public List<GeneratedScheduleViewModel> GeneratedSchedules { get; set; } = new();
        public string SerializedSchedules { get; set; } = "[]";
    }

    public class CourseSummary
    {
        public int Id { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public int SectionCount { get; set; }
    }

    public class GeneratedScheduleViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ClassSessionViewModel> Classes { get; set; } = new();
    }

    public class ClassSessionViewModel
    {
        public string CourseCode { get; set; } = string.Empty;
        public string SectionCode { get; set; } = string.Empty;
        public DayOfWeek Day { get; set; }
        public int StartHour { get; set; }
        public int Duration { get; set; } // hours
        public string TimeLabel { get; set; } = string.Empty; 
    }
}
