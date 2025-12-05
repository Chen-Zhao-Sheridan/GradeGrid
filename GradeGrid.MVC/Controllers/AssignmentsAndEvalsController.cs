using GradeGrid.MVC.DTOs;
using GradeGrid.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GradeGrid.MVC.Controllers
{
    public class AssignmentsAndEvalsController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly JsonSerializerOptions _jsonOptions;
        private const string ApiBaseUrl = "https://localhost:7233/api";

        public AssignmentsAndEvalsController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        // build the view model with all the eval items seperated into pages
        [HttpGet]
        public async Task<IActionResult> Index(int currentPage = 1, int? selectedItemId = null)
        {
            var client = _clientFactory.CreateClient();
            var model = new AssignmentsAndEvalsViewModel { CurrentPage = currentPage, SelectedItemId = selectedItemId };

            // fetch all evals 
            var evalResponse = await client.GetAsync($"{ApiBaseUrl}/Evaluations");
            if (evalResponse.IsSuccessStatusCode)
            {
                var json = await evalResponse.Content.ReadAsStringAsync();
                var allItems = JsonSerializer.Deserialize<List<EvaluationDto>>(json, _jsonOptions) ?? new List<EvaluationDto>();

                // sort by due date
                allItems = allItems.OrderBy(e => e.DueDate).ToList();

                // seperate by page and populate viewmodel
                model.TotalPages = (int)Math.Ceiling(allItems.Count / (double)model.PageSize);
                model.PaginatedItems = allItems.Skip((currentPage - 1) * model.PageSize).Take(model.PageSize).ToList();

                if (selectedItemId.HasValue)
                {
                    model.SelectedItem = allItems.FirstOrDefault(e => e.Id == selectedItemId.Value);
                }
                else if (model.PaginatedItems.Any())
                {
                    model.SelectedItem = model.PaginatedItems.First();
                    model.SelectedItemId = model.SelectedItem.Id;
                }
            }

            // fetch all courses this semester
            var (currentTerm, currentYear) = GetCurrentSemester();
            var courseResponse = await client.GetAsync($"{ApiBaseUrl}/Courses?term={(int)currentTerm}&year={currentYear}");
            if (courseResponse.IsSuccessStatusCode)
            {
                // populate viewmodel
                var json = await courseResponse.Content.ReadAsStringAsync();
                model.AvailableCourses = JsonSerializer.Deserialize<List<CourseSummaryDto>>(json, _jsonOptions) ?? new List<CourseSummaryDto>();
            }

            return View(model);
        }

        private (Term, int) GetCurrentSemester()
        {
            var today = DateTime.Now;
            int year = today.Year;
            Term term;

            if (today.Month <= 4) term = Term.Winter;       
            else if (today.Month <= 8) term = Term.Summer;  
            else term = Term.Fall;                          

            return (term, year);
        }

        // simple crud for evals
        [HttpPost]
        public async Task<IActionResult> Create(CreateEvaluationDto dto)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.PostAsJsonAsync($"{ApiBaseUrl}/Evaluations", dto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetEvaluation(int id)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"{ApiBaseUrl}/Evaluations/{id}");
            if (response.IsSuccessStatusCode) return Content(await response.Content.ReadAsStringAsync(), "application/json");
            else return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateEvaluationDto dto)
        {
            var client = _clientFactory.CreateClient();
            await client.PutAsJsonAsync($"{ApiBaseUrl}/Evaluations/{id}", dto);
            return RedirectToAction("Index", new { selectedItemId = id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _clientFactory.CreateClient();
            await client.DeleteAsync($"{ApiBaseUrl}/Evaluations/{id}");
            return RedirectToAction("Index");
        }
    }
}
