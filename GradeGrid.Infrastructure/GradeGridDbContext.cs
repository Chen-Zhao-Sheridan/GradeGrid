using GradeGrid.Core;
using GradeGrid.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GradeGrid.Infrastructure
{
    public class GradeGridDbContext : DbContext
    {
            
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<EvaluationItem> EvaluationItems => Set<EvaluationItem>();

        public GradeGridDbContext(DbContextOptions<GradeGridDbContext> options) : base(options) { }
    }
}
