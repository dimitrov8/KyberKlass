namespace KyberKlass.Data;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

public class KyberKlassDbContext : IdentityDbContext
{
	public KyberKlassDbContext(DbContextOptions<KyberKlassDbContext> options)
		: base(options)
	{
	}

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

		builder.Entity<TeacherSubject>()
			.HasKey(ts => new { ts.TeacherId, ts.SubjectId });

		base.OnModelCreating(builder);
	}
}