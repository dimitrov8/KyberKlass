using KyberKlass.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static KyberKlass.Data.Configurations.Constants.SeedDataConstants.Admin;

namespace KyberKlass.Data.Configurations;
public class IdentityUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("AspNetUsers");

        SeedAdminUser(builder);
    }

    private static void SeedAdminUser(EntityTypeBuilder<ApplicationUser> builder)
    {
        PasswordHasher<ApplicationUser> passwordHasher = new();
        string? hashedPassword = passwordHasher.HashPassword(null!, DEFAULT_ADMIN_PASSWORD);

        // Seeding the admin user
        builder.HasData(new ApplicationUser
        {
            Id = DefaultAdminUserId,
            UserName = DEFAULT_ADMIN_EMAIL,
            NormalizedUserName = DEFAULT_ADMIN_EMAIL.ToUpper(),
            Email = DEFAULT_ADMIN_EMAIL,
            NormalizedEmail = DEFAULT_ADMIN_EMAIL.ToUpper(),
            EmailConfirmed = false,
            FirstName = DEFAULT_ADMIN_FIRST_NAME,
            LastName = DEFAULT_ADMIN_LAST_NAME,
            BirthDate = DateTime.Parse(DEFAULT_ADMIN_BIRTH_DATE),
            Address = DEFAULT_ADMIN_ADDRESS,
            RoleId = AdminRoleId,
            PasswordHash = hashedPassword,
            SecurityStamp = DEFAULT_ADMIN_PUBLIC_STAMP,
            ConcurrencyStamp = DEFAULT_ADMIN_CONCURRENCYSTAMP,
            PhoneNumber = DEFAULT_ADMIN_PHONE_NUMBER,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnd = null,
            LockoutEnabled = false,
            AccessFailedCount = 0
        });
    }
}