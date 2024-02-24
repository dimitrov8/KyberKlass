namespace KyberKlass.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class ExamEntityConfiguration : IEntityTypeConfiguration<Exam>
{
	public void Configure(EntityTypeBuilder<Exam> builder)
	{
		builder
			.HasQueryFilter(e => !e.Classroom.IsDeleted);

		builder
			.HasKey(e => new { e.ClassroomId, e.SubjectId });

		builder
			.HasOne(e => e.Classroom)
			.WithMany(c => c.Exams)
			.HasForeignKey(e => e.ClassroomId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}