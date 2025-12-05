using Microsoft.AspNetCore.Mvc;
using GradeGrid.Core.Interfaces;
using GradeGrid.Core.Models;

namespace GradeGrid.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarEventRepository _repo;

        public CalendarController(ICalendarEventRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _repo.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Create(CalendarEvent evt)
        {
            await _repo.AddAsync(evt);
            return Ok(evt);
        }
        
    }
}
