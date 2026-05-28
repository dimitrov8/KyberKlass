#region

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static KyberKlass.Data.Configurations.Constants.SeedDataConstants.Admin;
using static KyberKlass.Data.Configurations.Constants.SeedDataConstants.Role;

#endregion

namespace KyberKlass.Data.Configurations;

public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
    {
        builder.ToTable("AspNetRoles");
        builder.Property(propertyExpression: static r => r.Name).HasMaxLength(30);
        builder.Property(propertyExpression: static r => r.NormalizedName).HasMaxLength(30);

        SeedRoles(builder);
    }

    private static void SeedRoles(EntityTypeBuilder<IdentityRole<Guid>> builder)
    {
        builder.HasData(
        new IdentityRole<Guid> { Id = AdminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
        new IdentityRole<Guid> { Id = TeacherRoleId, Name = "Teacher", NormalizedName = "TEACHER" },
        new IdentityRole<Guid> { Id = StudentRoleId, Name = "Student", NormalizedName = "STUDENT" },
        new IdentityRole<Guid> { Id = GuardianRoleId, Name = "Guardian", NormalizedName = "GUARDIAN" }
        );
    }
}