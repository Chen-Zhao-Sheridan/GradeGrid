using GradeGrid.Core.Enums;
using GradeGrid.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeGrid.Infrastructure
{
    // AI Generated Data Seeder
    public static class CourseDataSeeder
    {
        public static void Initialize(GradeGridDbContext context)
        {
            // 1. Ensure database is created
            context.Database.EnsureCreated();

            // 2. Look for any courses. If present, don't seed.
            if (context.Courses.Any())
            {
                return;   // DB has been seeded
            }

            var courses = new List<Course>();

            // --- DATA SET 1: WINTER 2025 (The main test set) ---

            // Course 1: Advanced .NET (3 Sections)
            var prog3000 = new Course
            {
                CourseCode = "PROG3000",
                Term = Term.Winter,
                Year = 2025,
                Sections = new List<Section>
                {
                    // Section 1: Mon/Wed Morning
                    new Section { SectionCode = "01", CourseId = 0, TimeSlots = new List<TimeSlot> {
                        new TimeSlot { Day = DayOfWeek.Monday, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(10, 0), SectionId = 0 },
                        new TimeSlot { Day = DayOfWeek.Wednesday, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(10, 0), SectionId = 0 }
                    }},
                    // Section 2: Mon/Wed Afternoon
                    new Section { SectionCode = "02", CourseId = 0, TimeSlots = new List<TimeSlot> {
                        new TimeSlot { Day = DayOfWeek.Monday, StartTime = new TimeOnly(12, 0), EndTime = new TimeOnly(14, 0), SectionId = 0 },
                        new TimeSlot { Day = DayOfWeek.Wednesday, StartTime = new TimeOnly(12, 0), EndTime = new TimeOnly(14, 0), SectionId = 0 }
                    }},
                    // Section 3: Tue/Thu Evening
                    new Section { SectionCode = "03", CourseId = 0, TimeSlots = new List<TimeSlot> {
                        new TimeSlot { Day = DayOfWeek.Tuesday, StartTime = new TimeOnly(18, 0), EndTime = new TimeOnly(20, 0), SectionId = 0 },
                        new TimeSlot { Day = DayOfWeek.Thursday, StartTime = new TimeOnly(18, 0), EndTime = new TimeOnly(20, 0), SectionId = 0 }
                    }}
                }
            };
            courses.Add(prog3000);

            // Course 2: Database Management (2 Sections)
            var dbas3000 = new Course
            {
                CourseCode = "DBAS3000",
                Term = Term.Winter,
                Year = 2025,
                Sections = new List<Section>
                {
                    // Section 1: Tue/Thu Morning (Clashes with nothing so far)
                    new Section { SectionCode = "01", CourseId = 0, TimeSlots = new List<TimeSlot> {
                        new TimeSlot { Day = DayOfWeek.Tuesday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(11, 0), SectionId = 0 },
                        new TimeSlot { Day = DayOfWeek.Thursday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(11, 0), SectionId = 0 }
                    }},
                    // Section 2: Mon/Wed Morning (Clashes with PROG3000 Sec 1)
                    new Section { SectionCode = "02", CourseId = 0, TimeSlots = new List<TimeSlot> {
                        new TimeSlot { Day = DayOfWeek.Monday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(11, 0), SectionId = 0 },
                        new TimeSlot { Day = DayOfWeek.Wednesday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(11, 0), SectionId = 0 }
                    }}
                }
            };
            courses.Add(dbas3000);

            // Course 3: Communications (Friday Only)
            var comm1000 = new Course
            {
                CourseCode = "COMM1000",
                Term = Term.Winter,
                Year = 2025,
                Sections = new List<Section>
                {
                    new Section { SectionCode = "A", CourseId = 0, TimeSlots = new List<TimeSlot> {
                        new TimeSlot { Day = DayOfWeek.Friday, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(11, 0), SectionId = 0 }
                    }},
                    new Section { SectionCode = "B", CourseId = 0, TimeSlots = new List<TimeSlot> {
                        new TimeSlot { Day = DayOfWeek.Friday, StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(16, 0), SectionId = 0 }
                    }}
                }
            };
            courses.Add(comm1000);

            // --- DATA SET 2: FALL 2024 (To test Term/Year dropdowns) ---

            var math1000 = new Course
            {
                CourseCode = "MATH1000",
                Term = Term.Fall,
                Year = 2024,
                Sections = new List<Section>
                {
                    new Section { SectionCode = "01", CourseId = 0, TimeSlots = new List<TimeSlot> {
                        new TimeSlot { Day = DayOfWeek.Monday, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(12, 0), SectionId = 0 }
                    }}
                }
            };
            courses.Add(math1000);

            // 3. Add to context and save
            context.Courses.AddRange(courses);
            context.SaveChanges();
        }
    }
}