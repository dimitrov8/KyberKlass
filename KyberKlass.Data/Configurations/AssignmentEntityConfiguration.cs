namespace KyberKlass.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class AssignmentEntityConfiguration : IEntityTypeConfiguration<Assignment>
{
	public void Configure(EntityTypeBuilder<Assignment> builder)
	{
		builder
			.HasQueryFilter(a => !a.Classroom.IsDeleted);

		builder
			.HasKey(a => new { a.ClassroomId, a.DueDate });

		builder
			.HasOne(a => a.Classroom)
			.WithMany(c => c.Assignments)
			.HasForeignKey(a => a.ClassroomId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}