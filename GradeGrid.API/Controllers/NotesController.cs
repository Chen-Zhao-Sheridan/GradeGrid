using Microsoft.AspNetCore.Mvc;
using GradeGrid.Core.Interfaces;
using GradeGrid.Core.Models;

namespace GradeGrid.API.Controllers
{
    public class NotesController : ControllerBase{
        private readonly INoteRepository _repo;

        public NotesController(INoteRepository repo)
        {
            _repo = repo;
        }        

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
        Ok(await _repo.GetAllAsync());

        public async Task<IActionResult> Create(Note note)
        {
            await _repo.AddAsync(note);
            return Ok(note);
        }

    }
}