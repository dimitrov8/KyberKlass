namespace KyberKlass.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class PraiseEntityConfiguration : IEntityTypeConfiguration<Praise>
{
	public void Configure(EntityTypeBuilder<Praise> builder)
	{
		builder
			.HasQueryFilter(p => !p.Classroom.IsDeleted);

		builder
			.HasKey(p => new { p.StudentId, p.Date });

		builder
			.HasOne(p => p.Classroom)
			.WithMany(c => c.Praises)
			.HasForeignKey(p => p.ClassroomId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}