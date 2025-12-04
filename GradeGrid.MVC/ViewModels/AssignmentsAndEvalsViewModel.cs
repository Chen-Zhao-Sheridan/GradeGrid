using GradeGrid.MVC.DTOs;
using System.Collections.Generic;

namespace GradeGrid.MVC.ViewModels
{
    public class AssignmentsAndEvalsViewModel
    {
        public List<CourseSummaryDto> AvailableCourses { get; set; } = new();
        public List<EvaluationDto> PaginatedItems { get; set; } = new();
        public EvaluationDto? SelectedItem { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int? SelectedItemId { get; set; }
        public int PageSize { get; set; } = 5;
    }
}
