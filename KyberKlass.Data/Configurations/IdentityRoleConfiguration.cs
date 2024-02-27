namespace KyberKlass.Data.Configurations;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
{
	public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
	{
		builder.ToTable("AspNetRoles");
		builder.Property(r => r.Name).HasMaxLength(30);
		builder.Property(r => r.NormalizedName).HasMaxLength(30);

		SeedRoles(builder);
	}

	private static void SeedRoles(EntityTypeBuilder<IdentityRole<Guid>> builder)
	{
		string[] roleNames = { "Admin", "Teacher", "Student", "Guardian" };

		foreach (string roleName in roleNames)
		{
			var role = new IdentityRole<Guid>
			{
				Id = Guid.NewGuid(),
				Name = roleName,
				NormalizedName = roleName.ToUpper()
			};

			builder.HasData(role);
		}
	}
}