using GradeGrid.Core.Models;
using GradeGrid.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GradeGrid.MVC.Controllers
{
    public class HomeController : Controller
    {
<<<<<<< HEAD
        /*
        private readonly ILogger<HomeController> _logger;
=======
            private readonly ILogger<HomeController> _logger;
            private readonly HttpClient _apiClient;
>>>>>>> a5909737e8306bc0972e496404a4708510c48a3e

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
            var evaluations = await _apiClient.GetFromJsonAsync<List<EvaluationItem>>("api/evaluationitems")?? new List<EvaluationItem>();
            var totalCount = evaluations.Count;
            var completedCount = evaluations.Count(e => e.Notes.Contains("Done", StringComparison.OrdinalIgnoreCase));
            var completionPercent = totalCount == 0 ? 0 : (int)Math.Round(completedCount * 100.0 / totalCount);

            var upcoming = evaluations
                .OrderBy(e => e.DueDate)
                .Take(5)
                .ToList();

            ViewBag.CompletionPercent = completionPercent;
            ViewBag.Upcoming = upcoming;

            return View(evaluations);
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
        */
    }
    
}
