﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static KyberKlass.Data.Configurations.Constants.SeedDataConstants.Admin;

namespace KyberKlass.Data.Configurations;
public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
    {
        builder.ToTable("AspNetUserRoles");

        SeedAdminUserRole(builder);
    }

    private void SeedAdminUserRole(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
    {
        try
        {
            builder.HasData(new IdentityUserRole<Guid>
            {
                UserId = DefaultAdminUserId,
                RoleId = AdminRoleId
            });
        }
        catch (Exception ex)
        {
            // Output error message to console
            Console.WriteLine($"Error occurred while seeding admin user role: {ex.Message}");
        }
    }
}