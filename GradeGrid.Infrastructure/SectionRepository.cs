using GradeGrid.Core.Models;
using GradeGrid.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradeGrid.Infrastructure
{
    public class SectionRepository : ISectionRepository
    {
        private GradeGridDbContext _context;
        public SectionRepository(GradeGridDbContext ctx)
        {
            _context = ctx;
        }

        public async Task<Section?> FindById(int id)
        {
            // Remember to actually include the nav properties
            return await _context.Sections
                .Include(s => s.TimeSlots)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task Delete(int sectionId)
        {
            var sectionToRemove = await FindById(sectionId);
            if (sectionToRemove != null)
            {
                _context.Sections.Remove(sectionToRemove);
                await _context.SaveChangesAsync();
            }
        }
    }
}
