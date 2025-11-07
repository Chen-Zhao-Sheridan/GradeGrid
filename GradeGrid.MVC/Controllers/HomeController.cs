using GradeGrid.Core.Models;
using GradeGrid.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GradeGrid.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult CoursePlanner()
        {
            List<Course> courseList = [
                new Course { CourseCode = "CS10001", Hour = 7, Day = DayOfWeek.Monday },
                new Course { CourseCode = "MATH20003", Hour = 9, Day = DayOfWeek.Monday },
                new Course { CourseCode = "ENGL15000", Hour = 13, Day = DayOfWeek.Monday },
                new Course { CourseCode = "PHYS11000", Hour = 9, Day = DayOfWeek.Wednesday },
                new Course { CourseCode = "CHEM20010", Hour = 11, Day = DayOfWeek.Thursday },
                new Course { CourseCode = "BIO10090", Hour = 14, Day = DayOfWeek.Thursday },
                new Course { CourseCode = "ART30000", Hour = 9, Day = DayOfWeek.Friday },
                new Course { CourseCode = "HIST20020", Hour = 12, Day = DayOfWeek.Friday },
                new Course { CourseCode = "PSYC100", Hour = 15, Day = DayOfWeek.Friday },
                ];

            var viewModel = new CoursePlannerViewModel() { Courses = courseList };

            return View(viewModel);
        }

        private readonly List<EvaluationItem> _allItems = new List<EvaluationItem>
        {
            new EvaluationItem { Id = 1, Title = "Assignment #1", Type = "Assignment", Description = "Info from the Assignments/Quiz Page Itself is in this area", RelevantResources = { "Link To Relevant Slate Resource #1", "Link To Relevant Slate Resource #2", "Link To Relevant Slate Resource #3" } },
            new EvaluationItem { Id = 2, Title = "Assignment #2", Type = "Assignment", Description = "This is the description for the second assignment.", RelevantResources = { "Link To Resource A", "Link To Resource B" } },
            new EvaluationItem { Id = 3, Title = "Quiz #1", Type = "Quiz", Description = "This is the first quiz.", RelevantResources = { "Link To Quiz Resource 1" } },
            new EvaluationItem { Id = 4, Title = "Quiz #2", Type = "Quiz", Description = "This is the second quiz." },
            new EvaluationItem { Id = 5, Title = "Assignment #3", Type = "Assignment", Description = "Description for assignment 3." },
            new EvaluationItem { Id = 6, Title = "Assignment #4", Type = "Assignment", Description = "Description for assignment 4." },
            new EvaluationItem { Id = 7, Title = "Quiz #3", Type = "Quiz", Description = "Description for quiz 3." },
            new EvaluationItem { Id = 8, Title = "Final Project", Type = "Assignment", Description = "Details about the final project.", RelevantResources = { "Project Guidelines", "Submission Portal Link" } }
        };

        public IActionResult AssignmentsAndEvals(int? selectedItemId, int currentPage = 1)
        {
            int pageSize = 4;

            var viewModel = new AssignmentsAndEvalsViewModel();

            if (!selectedItemId.HasValue && _allItems.Any())
            {
                selectedItemId = _allItems.First().Id;
            }

            viewModel.SelectedItemId = selectedItemId;
            viewModel.SelectedItem = _allItems.FirstOrDefault(item => item.Id == selectedItemId);

            viewModel.TotalPages = (int)System.Math.Ceiling(_allItems.Count / (double)pageSize);
            viewModel.CurrentPage = currentPage;

            viewModel.PaginatedItems = _allItems
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
