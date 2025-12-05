using Microsoft.AspNetCore.Mvc;
using GradeGrid.Core.Interfaces;
using GradeGrid.Core.Models;

namespace GradeGrid.MVC.Controllers
{
    public class CalendarViewController : Controller
    {
        private readonly ICalendarEventRepository _repo;

        public CalendarViewController(ICalendarEventRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Weekly()
        {
            var events = await _repo.GetAllAsync();
            return View("Calendar/Weekly", events);
        }

        public async Task<IActionResult> ViewEvent(int id)
        {
            var evt = await _repo.GetByIdAsync(id);
            if (evt == null) return NotFound();

            return View("Calendar/ViewEvent", evt);
        }

        public IActionResult CreateEvent()
        {
            return View("Calendar/CreateEvent");
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(CalendarEvent model)
        {
            await _repo.AddAsync(model);
            return RedirectToAction("Weekly");
        }

        public async Task<IActionResult> EditEvent(int id)
        {
            var evt = await _repo.GetByIdAsync(id);
            if (evt == null) return NotFound();

            return View("Calendar/EditEvent", evt);
        }

        [HttpPost]
        public async Task<IActionResult> EditEvent(CalendarEvent model)
        {
            await _repo.UpdateAsync(model);
            return RedirectToAction("Weekly");
        }

        public async Task<IActionResult> DeleteEvent(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction("Weekly");
        }
    }
}
