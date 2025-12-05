using GradeGrid.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeGrid.Infrastructure.Interfaces
{
    public interface ISectionRepository
    {
        Task<Section?> FindById(int id);
        Task Delete(int id);
    }
}
