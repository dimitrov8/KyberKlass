namespace KyberKlass.Data.Configurations;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using static Constants.SeedDataConstants.Admin;

public class IdentityUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
	public void Configure(EntityTypeBuilder<ApplicationUser> builder)
	{
		builder.ToTable("AspNetUsers");

		this.SeedAdminUser(builder);
	}

	private void SeedAdminUser(EntityTypeBuilder<ApplicationUser> builder)
	{
		var passwordHasher = new PasswordHasher<ApplicationUser>();
		string? hashedPassword = passwordHasher.HashPassword(null!, "admin");

		// Seeding the admin user
		builder.HasData(new ApplicationUser
		{
			Id = DefaultAdminUserId,
			UserName = DEFAULT_ADMIN_EMAIL,
			NormalizedUserName = DEFAULT_ADMIN_EMAIL.ToUpper(),
			Email = DEFAULT_ADMIN_EMAIL,
			NormalizedEmail = DEFAULT_ADMIN_EMAIL.ToUpper(),
			EmailConfirmed = false,
			FirstName = "Admin",
			LastName = "User",
			BirthDate = DateTime.Parse("01/01/2001"),
			Address = "1416 Ryan Mountains",
			PasswordHash = hashedPassword,
			SecurityStamp = "0DB9D047-3375-4739-9C32-217CC8337032",
			ConcurrencyStamp = "d4f6406d-9b51-4290-994d-cf1bb9668b5e",
			PhoneNumber = "08888888888",
			PhoneNumberConfirmed = false,
			TwoFactorEnabled = false,
			LockoutEnd = null,
			LockoutEnabled = false,
			AccessFailedCount = 0
		});
	}
}