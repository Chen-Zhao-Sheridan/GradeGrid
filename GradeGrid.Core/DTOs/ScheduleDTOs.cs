using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeGrid.Core.DTOs
{

    public class GeneratedScheduleDto
    {
        public int OptionNumber { get; set; }
        public List<ScheduleSectionDto> Sections { get; set; } = new List<ScheduleSectionDto>();
    }

    public class ScheduleSectionDto
    {
        public int Id { get; set; }
        public string SectionCode { get; set; }
        public string CourseCode { get; set; }
        public List<TimeSlotDto> TimeSlots { get; set; }
    }

    public class TimeSlotDto
    {
        public DayOfWeek Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
    
}
