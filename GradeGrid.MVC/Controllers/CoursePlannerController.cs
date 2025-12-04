using GradeGrid.Core.Enums;
using GradeGrid.MVC.FormModels;
using GradeGrid.MVC.ViewModels;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using System.Text;
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

        [HttpPost]
        public async Task<IActionResult> Generate(CoursePlannerViewModel model)
        {
            // this should be already generated from previous get, but dont trust user
            await RequestAndPopulateAvailableCourses(model);

            if (model.SelectedCourseIds == null || !model.SelectedCourseIds.Any())
            {
                ModelState.AddModelError("", "Please select at least one course.");
                return View("Index", model);
            }

            // make the list<int> of course id's and send to API POST
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(model.SelectedCourseIds),
                Encoding.UTF8,
                "application/json"
            );

            var client = _clientFactory.CreateClient();
            var response = await client.PostAsync($"{ApiBaseUrl}/Courses/generate_schedule", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                // loose coupling, rebuild Generated Schedule object
                var apiSchedules = JsonSerializer.Deserialize<List<GeneratedScheduleDto>>(content, _jsonOptions);

                if (apiSchedules != null)
                {
                    // map dtos -> viewmodels
                    model.GeneratedSchedules = apiSchedules.Select(MapToViewModel).ToList();
                    model.SerializedSchedules = JsonSerializer.Serialize(model.GeneratedSchedules, _jsonOptions);
                }
            }
            else
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"API Error: {errorMsg}");
            }

            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(CreateCourseFormModel form)
        {
            if (string.IsNullOrWhiteSpace(form.CourseCode))
            {
                return RedirectToAction("Index", new { error = "Course Code is required" });
            }

            // form is from html user space, need to transform into dto to send to api
            var apiDto = new CreateCourseDto
            {
                CourseCode = form.CourseCode,
                Year = form.Year,
                Term = form.Term,
                Sections = new List<CreateSectionDto>()
            };

            foreach (var secForm in form.Sections)
            {
                // skip empty sections
                if (string.IsNullOrWhiteSpace(secForm.SectionCode)) continue;

                var secDto = new CreateSectionDto
                {
                    SectionCode = secForm.SectionCode,
                    TimeSlots = new List<CreateTimeSlotDto>()
                };

                foreach (var slotForm in secForm.TimeSlots)
                {
                    // skip empty times
                    if (string.IsNullOrWhiteSpace(slotForm.StartTime) || string.IsNullOrWhiteSpace(slotForm.EndTime)) continue;

                    // html time is string, need to map to timeonly
                    if (TimeOnly.TryParse(slotForm.StartTime, out var start) && TimeOnly.TryParse(slotForm.EndTime, out var end))
                    {
                        secDto.TimeSlots.Add(
                            new CreateTimeSlotDto
                            {
                                Day = slotForm.Day,
                                StartTime = start,
                                EndTime = end
                            }
                        );
                    }
                }

                // if no timeslots, this is an empty section, dont add
                if (secDto.TimeSlots.Any()) apiDto.Sections.Add(secDto);
                
            }

            // if we have sections, this is a valid course, so call api and add it
            if (apiDto.Sections.Any())
            {
                // if the course is a valid course, send request to add to api

                var client = _clientFactory.CreateClient();
                var json = JsonSerializer.Serialize(apiDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{ApiBaseUrl}/Courses", content);
            }

            // reload page when done
            return RedirectToAction("Index", new { Year = form.Year, Term = form.Term });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMetadata(int id, string courseCode, int year, Term term)
        {
            var dto = new { CourseCode = courseCode, Year = year, Term = term };
            var client = _clientFactory.CreateClient();
            var response = await client.PutAsJsonAsync($"{ApiBaseUrl}/Courses/{id}", dto);

            return RedirectToAction("Index", new { year, term });
        }

        [HttpPost]
        public async Task<IActionResult> ReplaceSections(int courseId, CreateCourseFormModel form)
        {
            var client = _clientFactory.CreateClient();

            // get course from API
            var getResponse = await client.GetAsync($"{ApiBaseUrl}/Courses/{courseId}");
            if (!getResponse.IsSuccessStatusCode) return NotFound();

            var json = await getResponse.Content.ReadAsStringAsync();
            var existingCourse = JsonSerializer.Deserialize<CourseDto>(json, _jsonOptions);


            var sectionsToDelete = new List<int>();
            if (existingCourse != null && existingCourse.Sections != null)
            {
                foreach (var section in existingCourse.Sections)
                {
                    sectionsToDelete.Add(section.Id);
                }
            }


            // make new sections, same logic as in CreateCourse
            foreach (var secForm in form.Sections)
            {
                if (string.IsNullOrWhiteSpace(secForm.SectionCode)) continue;

                var apiSectionDto = new CreateSectionDto
                {
                    SectionCode = secForm.SectionCode,
                    TimeSlots = new List<CreateTimeSlotDto>()
                };

                foreach (var slotForm in secForm.TimeSlots)
                {
                    if (!string.IsNullOrWhiteSpace(slotForm.StartTime) && !string.IsNullOrWhiteSpace(slotForm.EndTime))
                    {
                        if (TimeOnly.TryParse(slotForm.StartTime, out var start) && TimeOnly.TryParse(slotForm.EndTime, out var end))
                        {
                            apiSectionDto.TimeSlots.Add(
                                new CreateTimeSlotDto
                                {
                                    Day = slotForm.Day,
                                    StartTime = start,
                                    EndTime = end
                                }
                            );
                        }
                    }
                }

                if (apiSectionDto.TimeSlots.Any()) await client.PostAsJsonAsync($"{ApiBaseUrl}/Courses/{courseId}/sections", apiSectionDto);
                
            }

            // delete after add
            foreach(var sectionToDeleteId in sectionsToDelete)
            {
                await client.DeleteAsync($"{ApiBaseUrl}/Sections/{sectionToDeleteId}");
            }

            return RedirectToAction("Index", new { year = form.Year, term = form.Term });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCourse(int courseId, int year, Term term)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.DeleteAsync($"{ApiBaseUrl}/Courses/{courseId}");
            return RedirectToAction("Index", new { year, term });
        }

        // map dto (deseralized json) to viewmodels for MVC frontend
        private GeneratedScheduleViewModel MapToViewModel(GeneratedScheduleDto dto)
        {
            var viewModel = new GeneratedScheduleViewModel
            {
                Id = dto.OptionNumber,
                Name = $"Option {dto.OptionNumber}"
            };

            // Dtos have Schedule -> Sections -> TimeSlots
            // this needs to be turned into a Day, Start, Duration for the viewmodel

            foreach (var section in dto.Sections)
            {
                foreach (var slot in section.TimeSlots)
                {
                    viewModel.Classes.Add(
                        new ClassSessionViewModel
                        {
                            CourseCode = section.CourseCode,
                            SectionCode = section.SectionCode,
                            Day = slot.Day,
                            StartHour = slot.StartTime.Hour,
                            Duration = slot.EndTime.Hour - slot.StartTime.Hour,
                            TimeLabel = $"{slot.StartTime:HH:mm} - {slot.EndTime:HH:mm}"
                        }
                    );
                }
            }

            return viewModel;
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
                    model.SerializedAvailableCourses = JsonSerializer.Serialize(courses, _jsonOptions);
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
            public int Id { get; set; }
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

        public class CreateCourseDto
        {
            public string CourseCode { get; set; } = string.Empty;
            public Term Term { get; set; }
            public int Year { get; set; }
            public List<CreateSectionDto> Sections { get; set; } = new();
        }

        public class CreateSectionDto
        {
            public string SectionCode { get; set; } = string.Empty;
            public List<CreateTimeSlotDto> TimeSlots { get; set; } = new();
        }

        public class CreateTimeSlotDto
        {
            public DayOfWeek Day { get; set; }

            // utilizing TimeOnly to match your API
            public TimeOnly StartTime { get; set; }
            public TimeOnly EndTime { get; set; }
        }
    }
}
