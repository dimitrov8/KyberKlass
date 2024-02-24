namespace KyberKlass.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class GuardianEntityConfiguration : IEntityTypeConfiguration<Guardian>
{
	public void Configure(EntityTypeBuilder<Guardian> builder)
	{
		builder
			.HasMany(g => g.Students)
			.WithOne(s => s.Guardian)
			.HasForeignKey(s => s.GuardianId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}