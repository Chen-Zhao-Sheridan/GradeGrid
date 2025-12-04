using GradeGrid.Core.DTOs;
using GradeGrid.Core.Models;
using GradeGrid.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GradeGrid.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluationsController : Controller
    {
        private readonly IEvaluationItemRepository _evaluationRepository;

        public EvaluationsController(IEvaluationItemRepository evaluationRepository)
        {
            _evaluationRepository = evaluationRepository;
        }

        // eg: api/evaluations?courseId=5 (gets should use query as to not need to send additional body info everytime)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EvaluationItem>>> GetEvaluations([FromQuery] int? courseId)
        {
            if (courseId.HasValue)
            {
                var evals = await _evaluationRepository.FindByCourseId(courseId.Value);
                return Ok(evals);
            }
            else
            {
                var allEvals = await _evaluationRepository.GetAll();
                return Ok(allEvals);
            }
        }

        [HttpPost]
        public async Task<ActionResult<EvaluationItem>> CreateEvaluation(CreateEvaluationDto dto)
        {
            var item = new EvaluationItem
            {
                Title = dto.Title,
                DueDate = dto.DueDate,
                Type = dto.Type,
                Notes = dto.Notes ?? string.Empty,
                CourseId = dto.CourseId
            };

            await _evaluationRepository.Add(item);
            return CreatedAtAction("CreateEvaluation", new { id = item.Id }, item);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EvaluationItem>> GetEvaluation(int id)
        {
            var item = await _evaluationRepository.FindById(id);
            if (item == null) return NotFound();
            else return Ok(item);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEvaluation(int id, UpdateEvaluationDto dto)
        {
            var item = await _evaluationRepository.FindById(id);
            if (item == null) return NotFound();
            else
            {
                item.Title = dto.Title ?? item.Title;
                item.DueDate = dto.DueDate ?? item.DueDate;
                item.Type = dto.Type ?? item.Type;
                item.Notes = dto.Notes ?? item.Notes;

                await _evaluationRepository.Update(item);
                return Ok(item);
            }

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvaluation(int id)
        {
            var item = await _evaluationRepository.FindById(id);
            if (item == null) return NotFound();
            else
            {
                await _evaluationRepository.Delete(id);
                return NoContent();
            }
        }
    }
}
