using GradeGrid.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeGrid.Core
{
    // for this to be a core service these dtos should be in core
    public class GeneratedScheduleDto
    {
        public int OptionNumber { get; set; }
        public List<ScheduleSectionDto> Sections { get; set; } = new List<ScheduleSectionDto>();
    }
    public class ScheduleSectionDto
    {
        public int Id { get; set; }
        public string SectionCode { get; set; }
        public string CourseCode { get; set; }
        public List<TimeSlotDto> TimeSlots { get; set; }
    }

    public class TimeSlotDto
    {
        public DayOfWeek Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }

    public class CourseScheduleGenerator : IScheduleGenerator
    {
        public List<GeneratedScheduleDto> GenerateValidSchedules(List<Course> courses)
        {
            var results = new List<GeneratedScheduleDto>();

            // flatten courses into sections
            var sectionsByCourse = courses
                .Select(c => c.Sections.ToList())
                .Where(s => s.Any())
                .ToList();

            if (!sectionsByCourse.Any())
                return results;

            // generate all possible combinations, picking 1 section per course

            IEnumerable<List<Section>> allCombinations = [[]];

            foreach (var courseSections in sectionsByCourse)
            {
                allCombinations = from seq in allCombinations
                                  from item in courseSections
                                  select seq.Concat([item]).ToList();
            }

            // only return combos that dont overlap
            int optionCounter = 1;
            foreach (var combo in allCombinations)
            {
                if (!HasOverlap(combo))
                {
                    var schedule = new GeneratedScheduleDto
                    {
                        OptionNumber = optionCounter,
                        Sections = combo.Select(s => 
                        new ScheduleSectionDto
                            {
                                Id = s.Id,
                                SectionCode = s.SectionCode,
                                CourseCode = s.Course?.CourseCode ?? "Unknown",
                                TimeSlots = s.TimeSlots.Select(ts => 
                                new TimeSlotDto
                                    {
                                        Day = ts.Day,
                                        StartTime = ts.StartTime,
                                        EndTime = ts.EndTime
                                    }
                                ).ToList()
                            }
                        ).ToList()
                    };

                    results.Add(schedule);
                    optionCounter++;
                }
            }

            return results;
        }

        private static bool HasOverlap(List<Section> sections)
        {
            // flatten timeslots from all sections
            var allSlots = sections.SelectMany(s => s.TimeSlots).ToList();

            // need index's to not double search space
            for (int i = 0; i < allSlots.Count; i++)
            {
                for (int j = i + 1; j < allSlots.Count; j++)
                {
                    var slotA = allSlots[i];
                    var slotB = allSlots[j];

                    // if same day, and times overlap, overlap detected
                    if (slotA.Day == slotB.Day &&
                        (slotA.StartTime < slotB.EndTime && slotA.EndTime > slotB.StartTime))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
