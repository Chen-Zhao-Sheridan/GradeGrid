using GradeGrid.Core.Interfaces;
using GradeGrid.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GradeGrid.Infrastructure
{
        public class NoteRepository : INoteRepository
    {
        private readonly GradeGridDbContext _context;

        public NoteRepository(GradeGridDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Note>> GetAllAsync() =>
            await _context.Notes.ToListAsync();

        public async Task<Note?> GetByIdAsync(int id) =>
            await _context.Notes.FindAsync(id);

        public async Task AddAsync(Note note)
        {
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Note note)
        {
            _context.Notes.Update(note);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var note = await GetByIdAsync(id);
            if (note != null)
            {
                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();
            }
        }
    }
}
