using GradeGrid.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeGrid.Infrastructure
{
    public interface IEvaluationItemRepository
    {
        List<EvaluationItem> GetAll();
        EvaluationItem? FindById(int Id);
        void Add(EvaluationItem request);
        void Delete(int requestId);
    }
}

