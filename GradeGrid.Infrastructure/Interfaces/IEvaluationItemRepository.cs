using GradeGrid.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeGrid.Infrastructure.Interfaces
{
    public interface IEvaluationItemRepository
    {
        Task<List<EvaluationItem>> GetAll();
        Task<EvaluationItem?> FindById(int Id);
        Task Add(EvaluationItem request);
        Task Update(EvaluationItem request);
        Task Delete(int requestId);
        Task<List<EvaluationItem>> FindByCourseId(int courseId);
    }
}

