namespace KyberKlass.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class StudentEntityConfiguration : IEntityTypeConfiguration<Student>
{
	public void Configure(EntityTypeBuilder<Student> builder)
	{
		builder
			.HasQueryFilter(s => s.ApplicationUser.IsActive);

		builder
			.HasMany(s => s.Absences)
			.WithOne(a => a.Student)
			.HasForeignKey(a => a.StudentId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasOne(s => s.Guardian)
			.WithMany(g => g.Students)
			.HasForeignKey(s => s.GuardianId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}