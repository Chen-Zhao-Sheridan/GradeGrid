using System.ComponentModel.DataAnnotations;

namespace GradeGrid.Core.Models
{
    public class CalendarEvent
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = "";
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string CourseName { get; set; } = "";
        public bool IsAcademic { get; set; }
        public string? Description { get; set; }
    }
}