using GradeGrid.Core.Interfaces;
using GradeGrid.Core.Models;
using Microsoft.EntityFrameworkCore;


namespace GradeGrid.Infrastructure
{
    public class CalendarEventRepository : ICalendarEventRepository
    {
        private readonly GradeGridDbContext _context;

        public CalendarEventRepository(GradeGridDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CalendarEvent>> GetAllAsync() =>
        await _context.CalendarEvents.ToListAsync();

        public async Task<CalendarEvent?> GetByIdAsync(int id) =>
            await _context.CalendarEvents.FindAsync(id);

        public async Task AddAsync(CalendarEvent evt)
        {
            _context.CalendarEvents.Add(evt);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CalendarEvent evt)
        {
            _context.CalendarEvents.Update(evt);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var evt = await GetByIdAsync(id);
            if (evt != null)
            {
                _context.CalendarEvents.Remove(evt);
                await _context.SaveChangesAsync();
            }
        }
    }
}