namespace KyberKlass.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

public class KyberKlassDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
	public KyberKlassDbContext(DbContextOptions<KyberKlassDbContext> options)
		: base(options)
	{
	}

	public DbSet<School> Schools { get; set; } = null!;

	public DbSet<Classroom> Classrooms { get; set; } = null!;

	public DbSet<Teacher> Teachers { get; set; } = null!;

	public DbSet<Student> Students { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder builder)
	{
		
		builder.Entity<Absence>()
			.HasKey(a => new { a.StudentId, a.Date });

		builder.Entity<Assignment>()
			.HasKey(a => new { a.ClassroomId, a.DueDate });

		builder.Entity<Behavior>()
			.HasKey(b => new { b.StudentId, b.Date });

		builder.Entity<Exam>()
			.HasKey(e => new { e.ClassroomId, e.SubjectId });

		builder.Entity<Praise>()
			.HasKey(p => new { p.StudentId, p.Date });

		builder.Entity<StudentGrade>()
			.HasKey(sg => new { sg.StudentId, sg.SubjectId });

		builder.Entity<Guardian>()
			.HasMany(g => g.Students)
			.WithOne(g => g.Guardian)
			.OnDelete(DeleteBehavior.Restrict);
			
		base.OnModelCreating(builder);
	}
}