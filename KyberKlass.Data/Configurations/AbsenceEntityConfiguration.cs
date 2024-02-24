namespace KyberKlass.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class AbsenceEntityConfiguration : IEntityTypeConfiguration<Absence>
{
	public void Configure(EntityTypeBuilder<Absence> builder)
	{
		builder
			.HasQueryFilter(a => !a.Classroom.IsDeleted);

		builder
			.HasKey(a => new { a.StudentId, a.Date });

		builder
			.HasOne(a => a.Classroom)
			.WithMany(c => c.Absences)
			.HasForeignKey(a => a.ClassroomId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasOne(a => a.Student)
			.WithMany()
			.HasForeignKey(a => a.StudentId)
			.OnDelete(DeleteBehavior.Restrict);

	}
}