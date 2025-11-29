using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeGrid.Core.Models
{
    public class TimeSlot
    {
        public required int Id { get; set; }
        public required DayOfWeek Day { get; set; }
        public required TimeSpan StartTime { get; set; }
        public required TimeSpan EndTime { get; set; }
        public required int SectionId { get; set; }
        public required Section Section { get; set; }
    }
}
