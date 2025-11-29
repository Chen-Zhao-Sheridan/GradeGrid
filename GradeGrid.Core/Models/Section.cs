using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeGrid.Core.Models
{
    public class Section
    {
        public required int Id { get; set; }
        public required string SectionCode { get; set; }
        public required int CourseId { get; set; }
        public required Course Course { get; set; }

        public ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
    }
}
