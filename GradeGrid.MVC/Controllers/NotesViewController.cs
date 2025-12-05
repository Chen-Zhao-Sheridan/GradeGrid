using Microsoft.AspNetCore.Mvc;
using GradeGrid.Core.Interfaces;
using GradeGrid.Core.Models;

namespace GradeGrid.MVC.Controllers
{
    public class NotesViewController : Controller
    {
        private readonly INoteRepository _repo;

        public NotesViewController(INoteRepository repo)
        {
            _repo = repo;
        }
        public async Task<IActionResult> NotesList()
        {
            var notes = await _repo.GetAllAsync();
            return View("Notes/NotesList", notes);
        }

        public async Task<IActionResult> ViewNotes(int id)
        {
            var note = await _repo.GetByIdAsync(id);
            if (note == null)
                return NotFound();

            return View("Notes/ViewNotes", note);
        }

        public IActionResult CreateNotes()
        {
            return View("Notes/CreateNotes");
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotes(Note model)
        {
            await _repo.AddAsync(model);
            return RedirectToAction("NotesList");
        }

        public async Task<IActionResult> EditNotes(int id)
        {
            var note = await _repo.GetByIdAsync(id);
            if (note == null)
                return NotFound();

            return View("Notes/EditNotes", note);
        }

        [HttpPost]
        public async Task<IActionResult> EditNotes(Note model)
        {
            await _repo.UpdateAsync(model);
            return RedirectToAction("NotesList");
        }

        public async Task<IActionResult> DeleteNotes(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction("NotesList");
        }
    }
}
