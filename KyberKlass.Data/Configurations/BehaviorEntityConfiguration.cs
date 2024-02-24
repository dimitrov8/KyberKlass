namespace KyberKlass.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class BehaviorEntityConfiguration : IEntityTypeConfiguration<Behavior>
{
	public void Configure(EntityTypeBuilder<Behavior> builder)
	{
		builder
			.HasQueryFilter(b => !b.Classroom.IsDeleted);

		builder
			.HasKey(b => new { b.StudentId, b.Date });

		builder
			.HasOne(b => b.Classroom)
			.WithMany(c => c.Behaviors)
			.HasForeignKey(b => b.ClassroomId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasOne(b => b.Student)
			.WithMany()
			.HasForeignKey(b => b.StudentId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}