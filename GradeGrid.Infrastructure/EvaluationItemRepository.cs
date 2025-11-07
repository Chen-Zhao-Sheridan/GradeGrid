using GradeGrid.Core.Models;

namespace GradeGrid.Infrastructure
{
    public class EvaluationItemRepository : IEvaluationItemRepository
    {
        private GradeGridDbContext _context;
        public EvaluationItemRepository(GradeGridDbContext ctx)
        {
            _context = ctx;
        }

        public void Add(EvaluationItem request)
        {
            _context.EvaluationItems.Add(request);
            _context.SaveChanges();
        }

        public void Delete(int evaluationItemId)
        {
            var evaluationItemToRemove = FindById(evaluationItemId);
            if (evaluationItemToRemove != null)
            {
                _context.EvaluationItems.Remove(evaluationItemToRemove);
                _context.SaveChanges();
            }
        }

        public EvaluationItem? FindById(int Id)
        {
            return _context.EvaluationItems.Find(Id);
        }

        public List<EvaluationItem> GetAll()
        {
            return _context.EvaluationItems.ToList();
        }
    }
}
