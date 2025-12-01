using GradeGrid.API.DTOs;
using GradeGrid.Core;
using GradeGrid.Core.Enums;
using GradeGrid.Core.Models;
using GradeGrid.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static GradeGrid.Core.CourseScheduleGenerator;

namespace GradeGrid.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IScheduleGenerator _scheduleGenerator;

        public CoursesController(ICourseRepository courseRepository, IScheduleGenerator scheduleGenerator)
        {
            _courseRepository = courseRepository;
            _scheduleGenerator = scheduleGenerator;
        }

        // eg: api/courses?term=Winter&year=2025 (gets should use query as to not need to send additional body info everytime)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses([FromQuery] Term term, [FromQuery] int year)
        {
            var courses = await _courseRepository.GetCoursesBySemester(term, year);
            return Ok(courses); // null ok here as its a collection
        }

        // post to api/courses includes a full course -> Section -> Timeslot data
        [HttpPost]
        public async Task<ActionResult<Course>> CreateCourse(CreateCourseDto dto)
        {
            bool exists = await _courseRepository.Exists(dto.CourseCode, dto.Term, dto.Year);
            if (exists) return Conflict($"Course '{dto.CourseCode}' for {dto.Term} {dto.Year} already exists");
            
            var course = new Course
            {
                CourseCode = dto.CourseCode,
                Term = dto.Term,
                Year = dto.Year
            };

            foreach (var secDto in dto.Sections)
            {
                var section = new Section
                {
                    SectionCode = secDto.SectionCode,
                    CourseId = 0, // set as FK by EF automaticaly
                    Course = course
                };

                foreach (var timeDto in secDto.TimeSlots)
                {
                    section.TimeSlots.Add(
                        new TimeSlot
                        {
                            Day = timeDto.Day,
                            StartTime = timeDto.StartTime,
                            EndTime = timeDto.EndTime,
                            SectionId = 0, // set as FK by EF automaticaly
                            Section = section
                        }
                    );
                }
                course.Sections.Add(section);
            }

            await _courseRepository.Add(course);
            return CreatedAtAction("CreateCourse", new { id = course.Id }, course);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _courseRepository.FindById(id);
            if (course == null) return NotFound();
            else return Ok(course);
        }

        // update is metadata only
        [HttpPut("{id}")] 
        public async Task<ActionResult> UpdateCourse(int id, UpdateCourseDto dto)
        {
            var course = await _courseRepository.FindById(id);
            if (course == null) return NotFound();
            else if (
                course.CourseCode == dto.CourseCode && 
                course.Term == dto.Term && 
                course.Year == dto.Year)
            {
                return Ok(course); // no change needed
            }
            else
            {
                bool exists = await _courseRepository.Exists(
                    dto.CourseCode ?? course.CourseCode,
                    dto.Term ?? course.Term,
                    dto.Year ?? course.Year
                    );
                if (exists) return Conflict($"Course '{dto.CourseCode}' for {dto.Term} {dto.Year} already exists");

                else
                {
                    course.CourseCode = dto.CourseCode ?? course.CourseCode;
                    course.Term = dto.Term ?? course.Term;
                    course.Year = dto.Year ?? course.Year;

                    await _courseRepository.Update(course);
                    return Ok(course);
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            var course = await _courseRepository.FindById(id);
            if (course == null) return NotFound();
            else
            {
                await _courseRepository.Delete(id);
                return NoContent();
            }

        }

        [HttpPost("{id}/sections")]
        public async Task<ActionResult> AddSection(int id, CreateSectionDto dto)
        {
            var course = await _courseRepository.FindById(id);
            if (course == null) return NotFound();
            else
            {
                var section = new Section
                {
                    CourseId = id,
                    SectionCode = dto.SectionCode
                };

                foreach (var t in dto.TimeSlots)
                {
                    section.TimeSlots.Add(
                        new TimeSlot
                        {
                            Day = t.Day,
                            StartTime = t.StartTime,
                            EndTime = t.EndTime,
                            SectionId = 0, // set as FK by EF automaticaly
                            Section = section
                        }
                    );
                }

                course.Sections.Add(section);
                await _courseRepository.Update(course);

                return Ok(section);
            }
        }

        [HttpPost("generate_schedule")]
        public async Task<ActionResult<List<GeneratedScheduleDto>>> GenerateSchedule(List<int> courseIds)
        {
            var courses = await _courseRepository.GetCoursesWithSections(courseIds);

            if (courses == null || !courses.Any()) return BadRequest("No valid courses found for the provided IDs.");
            else
            {
                var schedules = _scheduleGenerator.GenerateValidSchedules(courses);
                return Ok(schedules);
            }
        }
    }
}
