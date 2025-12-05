using GradeGrid.Core.Enums;
using GradeGrid.Core.Models;
using GradeGrid.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GradeGrid.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly List<EvaluationItem> _evaluations;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            _evaluations = new List<EvaluationItem>
            {
                new EvaluationItem
                {
                    Id = 1,
                    Title = "CS Assignment 1",
                    Type = EvaluationType.Assignment,
                    Notes = "Intro assignment",
                    DueDate = DateTime.Today.AddDays(3),
                    CourseId = 1
                },
                new EvaluationItem
                {
                    Id = 2,
                    Title = "Math Quiz 1",
                    Type = EvaluationType.Quiz,
                    Notes = "Chapters 1–3",
                    DueDate = DateTime.Today.AddDays(5),
                    CourseId = 2
                },
                new EvaluationItem
                {
                    Id = 3,
                    Title = "English Essay Plan",
                    Type = EvaluationType.Other,
                    Notes = "Topic proposal",
                    DueDate = DateTime.Today.AddDays(10),
                    CourseId = 3
                },
                new EvaluationItem
                {
                    Id = 4,
                    Title = "CS Quiz 1",
                    Type = EvaluationType.Quiz,
                    Notes = "Basics quiz",
                    DueDate = DateTime.Today.AddDays(1),
                    CourseId = 1
                }
            };
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TileNavigation()
        {
            return View();
        }

        public IActionResult Notes()
        {
            return View();
        }

        public IActionResult Calendar()
        {
            return View();
        }

        public IActionResult Analytics()
{
            var today = DateTime.Today;
            var next7Days = today.AddDays(7);
            var next14Days = today.AddDays(14);

            var total = _evaluations.Count;
            var overdue = _evaluations.Count(e => e.DueDate.Date < today);
            var thisWeek = _evaluations.Count(e => e.DueDate.Date >= today && e.DueDate.Date <= next7Days);
            var nextWeek = _evaluations.Count(e => e.DueDate.Date > next7Days && e.DueDate.Date <= next14Days);

            var itemsPerType = _evaluations
                .GroupBy(e => e.Type)
                .ToDictionary(g => g.Key, g => g.Count());

            var upcoming = _evaluations
                .Where(e => e.DueDate.Date >= today)
                .OrderBy(e => e.DueDate)
                .Take(5)
                .ToList();

            var model = new AnalyticsViewModel
            {
                TotalItems = total,
                OverdueItems = overdue,
                DueThisWeek = thisWeek,
                DueNextWeek = nextWeek,
                ItemsPerType = itemsPerType,
                UpcomingItems = upcoming
            };

            return View(model);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
