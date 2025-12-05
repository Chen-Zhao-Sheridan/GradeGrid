using GradeGrid.Core.Enums;
using GradeGrid.Core.Models;
using System.Collections.Generic;

namespace GradeGrid.MVC.ViewModels
{
    public class AnalyticsViewModel
    {
        public int TotalItems { get; set; }
        public int OverdueItems { get; set; }
        public int DueThisWeek { get; set; }
        public int DueNextWeek { get; set; }

        public Dictionary<EvaluationType, int> ItemsPerType { get; set; } = new();

        public List<EvaluationItem> UpcomingItems { get; set; } = new();
    }
}
