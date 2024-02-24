namespace KyberKlass.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class TeacherSubjectEntityConfiguration : IEntityTypeConfiguration<TeacherSubject>
{
	public void Configure(EntityTypeBuilder<TeacherSubject> builder)
	{
		builder
			.HasKey(ts => new { ts.TeacherId, ts.SubjectId });

		builder
			.HasOne(ts => ts.Teacher)
			.WithMany(t => t.TeachingSubjects)
			.HasForeignKey(ts => ts.TeacherId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasOne(ts => ts.Subject)
			.WithMany()
			.HasForeignKey(ts => ts.SubjectId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.Navigation(ts => ts.Teacher)
			.UsePropertyAccessMode(PropertyAccessMode.Property)
			.IsRequired(false);
	}
}