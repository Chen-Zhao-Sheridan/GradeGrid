using GradeGrid.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace GradeGrid.API.DTOs
{
    public class CreateCourseDto
    {
        [Required(ErrorMessage = "Course Code is required")]
        public string CourseCode { get; set; } = string.Empty; 

        [Required(ErrorMessage = "Course Term is required")]
        public Term Term { get; set; }

        [Required, Range(2020, 2100, ErrorMessage = "Please enter a valid year")]
        public int Year { get; set; }

        [Required, MinLength(1, ErrorMessage = "A course must have at least one section")]
        public List<CreateSectionDto> Sections { get; set; } = new();
    }

    public class CreateSectionDto
    {
        [Required(ErrorMessage = "Section Code is required")]
        public string SectionCode { get; set; } = string.Empty; 

        [Required, MinLength(1, ErrorMessage = "A section must have at least one timeslot")]
        public List<CreateTimeSlotDto> TimeSlots { get; set; } = new();
    }

    public class CreateTimeSlotDto
    {
        [Required]
        public DayOfWeek Day { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }
    }

    // metadata only, sections should be deleted and remade
    // supporting "updating" for sections/timeslots can be faked in frontend for much less work imo
    public class UpdateCourseDto
    {
        public string? CourseCode { get; set; }
        public Term? Term { get; set; }

        [Range(2020, 2100)]
        public int? Year { get; set; }
    }
}
