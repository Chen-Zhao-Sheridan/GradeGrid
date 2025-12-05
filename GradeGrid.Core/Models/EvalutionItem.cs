using GradeGrid.Core.Enums;

namespace GradeGrid.Core.Models
{
    public class EvaluationItem
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required EvaluationType Type { get; set; }
        public string Notes { get; set; } = string.Empty;
        public required int CourseId { get; set; }
        public Course? Course { get; set; }
        public required DateTime DueDate { get; set; }
    }
}