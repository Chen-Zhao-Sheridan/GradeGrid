namespace GradeGrid.MVC.DTOs
{
    // Duplicated MVC DTOs for loose coupling
    public class EvaluationDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public EvaluationType Type { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public int CourseId { get; set; }
        public string CourseCode { get; set; } = string.Empty; 
    }

    public class CourseSummaryDto
    {
        public int Id { get; set; }
        public string CourseCode { get; set; }
    }

    public class CreateEvaluationDto
    {
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public EvaluationType Type { get; set; }
        public string Notes { get; set; }
        public int CourseId { get; set; }
    }

    public class UpdateEvaluationDto
    {
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public EvaluationType Type { get; set; }
        public string Notes { get; set; }
    }

    public enum EvaluationType { Assignment = 0, Quiz = 1, Test = 2, Project = 3 }
}
