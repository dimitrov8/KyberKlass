namespace KyberKlass.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class TeacherEntityConfiguration : IEntityTypeConfiguration<Teacher>
{
	public void Configure(EntityTypeBuilder<Teacher> builder)
	{
		builder
			.HasQueryFilter(t => t.IsWorking);

		builder
			.HasMany(t => t.TeachingSubjects)
			.WithOne(ts => ts.Teacher)
			.OnDelete(DeleteBehavior.Cascade);

		builder
			.Navigation(t => t.TeachingSubjects)
			.UsePropertyAccessMode(PropertyAccessMode.Property);
	}
}