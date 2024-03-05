namespace KyberKlass.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class ClassroomEntityConfiguration : IEntityTypeConfiguration<Classroom>
{
	public void Configure(EntityTypeBuilder<Classroom> builder)
	{
		builder
			.HasQueryFilter(c => c.IsActive);

		builder
			.HasMany(c => c.Students)
			.WithOne(s => s.Classroom)
			.HasForeignKey(s => s.ClassroomId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasMany(c => c.Subjects)
			.WithOne(s => s.Classroom)
			.HasForeignKey(s => s.ClassroomId)
			.OnDelete(DeleteBehavior.Restrict);

    }
}