using GradeGrid.Core.Models;
using GradeGrid.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GradeGrid.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluationItemsController : Controller
    {
        private readonly IEvaluationItemRepository _evaluationRepository;

        public EvaluationItemsController(IEvaluationItemRepository evaluationRepository)
        {
            _evaluationRepository = evaluationRepository;
        }

        // GET: api/evaluationitems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EvaluationItem>>> GetAll()
        {
            var items = await _evaluationRepository.GetAll();
            return Ok(items);
        }

        // GET: api/evaluationitems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EvaluationItem>> GetById(int id)
        {
            var item = await _evaluationRepository.FindById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // GET: api/evaluationitems/bycourse/3
        [HttpGet("bycourse/{courseId}")]
        public async Task<ActionResult<IEnumerable<EvaluationItem>>> GetByCourse(int courseId)
        {
            var items = await _evaluationRepository.FindByCourseId(courseId);
            return Ok(items);
        }
    }
}
