using GradeGrid.Core.Models;
using System.Collections.Generic;

namespace GradeGrid.MVC.ViewModels
{
    public class AssignmentsAndEvalsViewModel
    {
        public List<EvaluationItem> PaginatedItems { get; set; }
        public EvaluationItem SelectedItem { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int? SelectedItemId { get; set; }
    }
}
