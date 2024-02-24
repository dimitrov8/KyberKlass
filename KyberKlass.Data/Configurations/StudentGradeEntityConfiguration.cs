namespace KyberKlass.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class StudentGradeEntityConfiguration : IEntityTypeConfiguration<StudentGrade>
{
	public void Configure(EntityTypeBuilder<StudentGrade> builder)
	{
		builder
			.HasKey(sg => new { sg.StudentId, sg.SubjectId });

		builder
			.HasQueryFilter(sg =>  sg.Classroom.IsDeleted);

		builder
			.HasOne(sg => sg.Classroom)
			.WithMany(c => c.Grades)
			.HasForeignKey(c => c.ClassroomId);
	}
}