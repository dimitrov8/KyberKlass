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
			.HasOne(s => s.Guardian)
			.WithMany(g => g.Students)
			.HasForeignKey(s => s.GuardianId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasOne(s => s.School)
			.WithMany(g => g.Students)
			.HasForeignKey(s => s.SchoolId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}