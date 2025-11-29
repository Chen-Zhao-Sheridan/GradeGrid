using GradeGrid.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeGrid.Core.Models
{
    public class Course
    {
        public required int Id { get; set; }
        public required string CourseCode { get; set; }
        public Term Term { get; set; }
        public int Year { get; set; }

        // Navigation Properties
        public ICollection<Section> Sections { get; set; } = new List<Section>();
        public ICollection<EvaluationItem> Evaluations { get; set; } = new List<EvaluationItem>();
    }
}
