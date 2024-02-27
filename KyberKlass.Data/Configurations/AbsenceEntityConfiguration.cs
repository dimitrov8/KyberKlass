namespace KyberKlass.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class AbsenceEntityConfiguration : IEntityTypeConfiguration<Absence>
{
	public void Configure(EntityTypeBuilder<Absence> builder)
	{
		builder
			.HasQueryFilter(a => !a.Student.IsEnrolled);
	}
}