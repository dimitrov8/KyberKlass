namespace KyberKlass.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class ClassroomEntityConfiguration : IEntityTypeConfiguration<Classroom>
{
	public void Configure(EntityTypeBuilder<Classroom> builder)
	{
		builder
			.HasMany(c => c.Students)
			.WithOne(s => s.Classroom)
			.HasForeignKey(s => s.ClassroomId)
			.OnDelete(DeleteBehavior.Cascade);

		builder
			.HasMany(c => c.Absences)
			.WithOne(a => a.Classroom)
			.HasForeignKey(a => a.ClassroomId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasMany(c => c.Praises)
			.WithOne(p => p.Classroom)
			.HasForeignKey(p => p.ClassroomId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasMany(c => c.Assignments)
			.WithOne(a => a.Classroom)
			.HasForeignKey(a => a.ClassroomId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasMany(c => c.Exams)
			.WithOne(e => e.Classroom)
			.HasForeignKey(e => e.ClassroomId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasMany(c => c.Behaviors)
			.WithOne(b => b.Classroom)
			.HasForeignKey(b => b.ClassroomId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}