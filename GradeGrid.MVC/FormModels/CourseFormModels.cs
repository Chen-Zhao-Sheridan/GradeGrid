using GradeGrid.MVC.DTOs;

namespace GradeGrid.MVC.FormModels
{
    public class CreateCourseFormModel
    {
        public string CourseCode { get; set; } = string.Empty;
        public int Year { get; set; }
        public Term Term { get; set; }

        public List<SectionFormModel> Sections { get; set; }

        public CreateCourseFormModel() // MVC hardcode to 5
        {
            Sections = new List<SectionFormModel>();
            for (int i = 0; i < 5; i++)
            {
                Sections.Add(new SectionFormModel());
            }
        }
    }

    public class SectionFormModel
    {
        public string? SectionCode { get; set; } 
        public List<TimeSlotFormModel> TimeSlots { get; set; }

        public SectionFormModel() // MVC hardcode to 2
        {
            TimeSlots = new List<TimeSlotFormModel>();
            for (int i = 0; i < 2; i++)
            {
                TimeSlots.Add(new TimeSlotFormModel());
            }
        }
    }

    public class TimeSlotFormModel
    {
        public DayOfWeek Day { get; set; }
        public string? StartTime { get; set; } // HTML time is a string
        public string? EndTime { get; set; }
    }
}
