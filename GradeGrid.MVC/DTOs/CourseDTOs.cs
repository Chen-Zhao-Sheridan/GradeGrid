namespace GradeGrid.MVC.DTOs
{
    // Duplicated MVC DTOs for loose coupling
    public class CourseDto
    {
        public int Id { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public List<SectionDto> Sections { get; set; } = new();
    }
    public class SectionDto
    {
        public int Id { get; set; }
        public string SectionCode { get; set; } = string.Empty;
        public List<TimeSlotDto> TimeSlots { get; set; } = new();
    }
    public class TimeSlotDto
    {
        public DayOfWeek Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }

    public class GeneratedScheduleDto
    {
        public int OptionNumber { get; set; }
        public List<ScheduleSectionDto> Sections { get; set; } = new();
    }

    public class ScheduleSectionDto
    {
        public int Id { get; set; }
        public string SectionCode { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public List<TimeSlotDto> TimeSlots { get; set; } = new();
    }

    public class CreateCourseDto
    {
        public string CourseCode { get; set; } = string.Empty;
        public Term Term { get; set; }
        public int Year { get; set; }
        public List<CreateSectionDto> Sections { get; set; } = new();
    }

    public class CreateSectionDto
    {
        public string SectionCode { get; set; } = string.Empty;
        public List<CreateTimeSlotDto> TimeSlots { get; set; } = new();
    }

    public class CreateTimeSlotDto
    {
        public DayOfWeek Day { get; set; }

        // utilizing TimeOnly to match your API
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }

    public enum Term
    {
        Winter,
        Summer,
        Fall
    }
}
