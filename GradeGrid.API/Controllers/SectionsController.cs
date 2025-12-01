using GradeGrid.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GradeGrid.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionsController : Controller
    {
        private readonly ISectionRepository _sectionRepository;

        public SectionsController(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        // only delete here as adding and updating should only be in relation to a course, in course controller
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSection(int id)
        {
            var section = await _sectionRepository.FindById(id);
            if (section == null) return NotFound();
            else
            {
                await _sectionRepository.Delete(id);
                return NoContent();
            }
        }
    }
}
