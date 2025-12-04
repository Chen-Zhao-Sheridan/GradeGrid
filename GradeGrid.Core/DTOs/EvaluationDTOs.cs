using GradeGrid.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace GradeGrid.Core.DTOs
{
    public class CreateEvaluationDto
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Due Date is required")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Evaluation Type is required")]
        public EvaluationType Type { get; set; }

        public string? Notes { get; set; }

        [Required(ErrorMessage = "Course ID is required")]
        public int CourseId { get; set; }
    }

    public class UpdateEvaluationDto
    {
        public string? Title { get; set; }
        public DateTime? DueDate { get; set; }
        public EvaluationType? Type { get; set; }
        public string? Notes { get; set; }
    }
}
