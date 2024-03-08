namespace KyberKlass.Data.Configurations;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Constants.SeedDataConstants.Admin;

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

		IdentityRole<Guid>[] roles = roleNames.Select((roleName, index) => new IdentityRole<Guid>
		{
			Id = roleName == "Admin" ? AdminRoleId : Guid.NewGuid(),
			Name = roleName,
			NormalizedName = roleName.ToUpper()
		}).ToArray();

		try
		{
			builder.HasData(roles);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error occurred while seeding roles: {ex.Message}");
		}
	}
}