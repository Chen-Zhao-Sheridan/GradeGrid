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
        public string CourseCode { get; set; }
        public int Hour { get; set; }
        public DayOfWeek Day { get; set; }
    }
}
