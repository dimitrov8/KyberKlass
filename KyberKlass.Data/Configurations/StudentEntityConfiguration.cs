namespace KyberKlass.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class StudentEntityConfiguration : IEntityTypeConfiguration<Student>
{
	public void Configure(EntityTypeBuilder<Student> builder)
	{
		builder
			.HasQueryFilter(s => s.IsEnrolled);
		
		builder
			.HasQueryFilter(s => !s.Classroom.IsDeleted);

		builder
			.HasOne(s => s.Guardian)
			.WithMany(g => g.Students)
			.HasForeignKey(s => s.GuardianId)
			.OnDelete(DeleteBehavior.Restrict);

	builder
		.HasOne(s => s.Classroom)
		.WithMany(c => c.Students)
		.HasForeignKey(s => s.ClassroomId)
		.OnDelete(DeleteBehavior.Restrict);
	}
}