namespace KyberKlass.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class GradeEntityConfiguration : IEntityTypeConfiguration<Grade>
{
	public void Configure(EntityTypeBuilder<Grade> builder)
	{
		builder
			.HasQueryFilter(g => g.Student.IsEnrolled);

		builder
			.HasOne(g => g.Student)
			.WithMany(s => s.Grades)
			.HasForeignKey(g => g.StudentId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasOne(g => g.Subject)
			.WithMany()
			.HasForeignKey(g => g.SubjectId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}