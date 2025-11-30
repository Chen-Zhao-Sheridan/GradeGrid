using GradeGrid.Core.Models;
using GradeGrid.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GradeGrid.Infrastructure
{
    public class EvaluationItemRepository : IEvaluationItemRepository
    {
        private GradeGridDbContext _context;
        public EvaluationItemRepository(GradeGridDbContext ctx)
        {
            _context = ctx;
        }

        public async Task Add(EvaluationItem item)
        {
            await _context.EvaluationItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(EvaluationItem item)
        {
            _context.EvaluationItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int evaluationItemId)
        {
            var evaluationItemToRemove = await FindById(evaluationItemId);
            if (evaluationItemToRemove != null)
            {
                _context.EvaluationItems.Remove(evaluationItemToRemove);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<EvaluationItem?> FindById(int Id)
        {
            return await _context.EvaluationItems
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == Id);
        }

        public async Task<List<EvaluationItem>> GetAll()
        {
            return await _context.EvaluationItems
                .Include(e => e.Course)
                .OrderBy(e => e.DueDate)
                .ToListAsync();
        }

        public async Task<List<EvaluationItem>> FindByCourseId(int courseId)
        {
            return await _context.EvaluationItems
                .Where(e => e.CourseId == courseId)
                .OrderBy(e => e.DueDate)
                .ToListAsync();
        }
    }
}
