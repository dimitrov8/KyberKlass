namespace KyberKlass.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class SchoolEntityConfiguration : IEntityTypeConfiguration<School>
{
	public void Configure(EntityTypeBuilder<School> builder)
	{
		builder
			.HasQueryFilter(c => !c.IsDeleted);

		builder
			.HasMany(s => s.Classrooms)
			.WithOne(c => c.School)
			.HasForeignKey(c => c.SchoolId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}