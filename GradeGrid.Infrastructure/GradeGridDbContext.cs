using GradeGrid.Core;
using GradeGrid.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GradeGrid.Infrastructure
{
    public class GradeGridDbContext : DbContext
    {

        public DbSet<Course> Courses { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<EvaluationItem> EvaluationItems { get; set; }

        public GradeGridDbContext(DbContextOptions<GradeGridDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // CourseCode + Term + Year must be unique (composite key)
            modelBuilder
                .Entity<Course>()
                .HasIndex(c => new { c.CourseCode, c.Term, c.Year })
                .IsUnique();

            // cascade delete section if course is deleted (one-many : course-section)
            modelBuilder.Entity<Section>()
                .HasOne(s => s.Course)
                .WithMany(c => c.Sections)
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // same thing with timeslots  (one-many : section-timeslots)
            modelBuilder.Entity<TimeSlot>()
                .HasOne(t => t.Section)
                .WithMany(s => s.TimeSlots)
                .HasForeignKey(t => t.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            // cascade delete evalitem if course is deleted (one-many : course-evalitem)
            modelBuilder.Entity<EvaluationItem>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Evaluations)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
