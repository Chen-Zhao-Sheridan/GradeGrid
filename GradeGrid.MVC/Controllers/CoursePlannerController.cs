using GradeGrid.Core.Enums;
using GradeGrid.MVC.ViewModels;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GradeGrid.MVC.Controllers
{
    public class CoursePlannerController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly JsonSerializerOptions _jsonOptions;
        private const string ApiBaseUrl = "https://localhost:7233/api";

        public CoursePlannerController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? year, Term? term)
        {
            // default is current semester
            var targetYear = year ?? DateTime.Now.Year;
            var targetTerm = term ?? GetCurrentTerm();

            var model = new CoursePlannerViewModel
            {
                Year = targetYear,
                Term = targetTerm
            };

            // populate the sidebar list
            await RequestAndPopulateAvailableCourses(model);

            return View(model);
        }

        private Term GetCurrentTerm()
        {
            int month = DateTime.Now.Month;
            if (month >= 1 && month <= 4) return Term.Winter;
            if (month >= 5 && month <= 8) return Term.Summer;
            return Term.Fall;
        }

        private async Task RequestAndPopulateAvailableCourses(CoursePlannerViewModel model)
        {
            var client = _clientFactory.CreateClient();

            // GET API request for all courses in a semester
            var response = await client.GetAsync($"{ApiBaseUrl}/Courses?term={(int)model.Term}&year={model.Year}");

            if (response.IsSuccessStatusCode)
            {
                // loose coupling, rebuild objects from json
                var content = await response.Content.ReadAsStringAsync();
                var courses = JsonSerializer.Deserialize<List<CourseDto>>(content, _jsonOptions);

                if (courses != null)
                {
                    // populate AvailableCourses with CourseSummary's
                    model.AvailableCourses = courses.Select(c => 
                    new CourseSummary
                        {
                            Id = c.Id,
                            CourseCode = c.CourseCode,
                            SectionCount = c.Sections?.Count ?? 0
                        }
                    ).ToList();
                }
            }
        }

        // Duplicated MVC DTOs for loose coupling
        public class CourseDto
        {
            public int Id { get; set; }
            public string CourseCode { get; set; } = string.Empty;
            public List<SectionDto> Sections { get; set; } = new();
        }
        public class SectionDto
        {
            public string SectionCode { get; set; } = string.Empty;
            public List<TimeSlotDto> TimeSlots { get; set; } = new();
        }
        public class TimeSlotDto
        {
            public DayOfWeek Day { get; set; }
            public TimeOnly StartTime { get; set; }
            public TimeOnly EndTime { get; set; }
        }

        public class GeneratedScheduleDto
        {
            public int OptionNumber { get; set; }
            public List<ScheduleSectionDto> Sections { get; set; } = new();
        }

        public class ScheduleSectionDto
        {
            public int Id { get; set; }
            public string SectionCode { get; set; } = string.Empty;
            public string CourseCode { get; set; } = string.Empty;
            public List<TimeSlotDto> TimeSlots { get; set; } = new();
        }
    }
}
