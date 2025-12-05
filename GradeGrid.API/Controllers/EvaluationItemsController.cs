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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EvaluationItem>>> GetAll()
        {
            var items = await _evaluationRepository.GetAll();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EvaluationItem>> GetById(int id)
        {
            var item = await _evaluationRepository.FindById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<EvaluationItem>> Create([FromBody] EvaluationItem request)
        {
            await _evaluationRepository.Add(request);
            return CreatedAtAction(
                nameof(GetById),
                new { id = request.Id },
                request
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EvaluationItem>> Update(int id, [FromBody] EvaluationItem request)
        {
            var existing = await _evaluationRepository.FindById(id);
            if (existing == null) return NotFound();

            existing.Title = request.Title;
            existing.Type = request.Type;
            existing.Notes = request.Notes;
            existing.DueDate = request.DueDate;
            existing.CourseId = request.CourseId;

            await _evaluationRepository.Update(existing);
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existing = await _evaluationRepository.FindById(id);
            if (existing == null) return NotFound();

            await _evaluationRepository.Delete(id);
            return NoContent();
        }
    }
}
