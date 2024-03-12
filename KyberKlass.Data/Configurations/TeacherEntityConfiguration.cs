namespace KyberKlass.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class TeacherEntityConfiguration : IEntityTypeConfiguration<Teacher>
{
	public void Configure(EntityTypeBuilder<Teacher> builder)
	{
		builder
			.HasQueryFilter(t => t.IsActive);

		builder
			.HasMany(t => t.Subjects)
			.WithOne(s => s.Teacher)
			.HasForeignKey(s => s.TeacherId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}