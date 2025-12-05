namespace GradeGrid.Core.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string ClassTitle { get; set; } = "";
        public string Topic { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
