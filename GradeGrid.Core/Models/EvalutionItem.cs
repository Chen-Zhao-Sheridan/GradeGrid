namespace GradeGrid.Core.Models
{
    public class EvaluationItem
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Type { get; set; }
        public string? Description { get; set; }
        public List<string> RelevantResources { get; set; } = new List<string>();
    }
}