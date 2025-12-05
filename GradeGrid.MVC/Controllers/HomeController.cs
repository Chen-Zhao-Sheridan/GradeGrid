using GradeGrid.Core.Models;
using GradeGrid.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GradeGrid.MVC.Controllers
{
    public class HomeController : Controller
    {

        
        private readonly ILogger<HomeController> _logger;

        private readonly HttpClient _apiClient;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _apiClient = httpClientFactory.CreateClient("GradeGridApi");
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

        public async Task<IActionResult> Analytics()
        {
            var evaluations = await _apiClient
                .GetFromJsonAsync<List<EvaluationItem>>("api/evaluationitems")
                ?? new List<EvaluationItem>();

            var today = DateTime.Today;
            var next7Days = today.AddDays(7);
            var next14Days = today.AddDays(14);

            var total = evaluations.Count;
            var overdue = evaluations.Count(e => e.DueDate.Date < today);
            var thisWeek = evaluations.Count(e => e.DueDate.Date >= today && e.DueDate.Date <= next7Days);
            var nextWeek = evaluations.Count(e => e.DueDate.Date > next7Days && e.DueDate.Date <= next14Days);

            var itemsPerType = evaluations
                .GroupBy(e => e.Type)
                .ToDictionary(g => g.Key, g => g.Count());

            var upcoming = evaluations
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
