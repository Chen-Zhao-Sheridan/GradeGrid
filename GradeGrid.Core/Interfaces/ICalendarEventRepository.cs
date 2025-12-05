using GradeGrid.Core.Models;

namespace GradeGrid.Core.Interfaces
{
    public interface ICalendarEventRepository
    {
        Task<IEnumerable<CalendarEvent>> GetAllAsync();
        Task<CalendarEvent?> GetByIdAsync(int id);
        Task AddAsync(CalendarEvent evt);
        Task UpdateAsync(CalendarEvent evt);
        Task DeleteAsync(int id);
    }
}
