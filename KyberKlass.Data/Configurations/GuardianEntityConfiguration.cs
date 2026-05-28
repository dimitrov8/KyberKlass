#region

using KyberKlass.Data.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static KyberKlass.Data.Configurations.Constants.SeedDataConstants.Guardian;

#endregion

namespace KyberKlass.Data.Configurations;

public class GuardianEntityConfiguration : IEntityTypeConfiguration<Guardian>
{
    public void Configure(EntityTypeBuilder<Guardian> builder)
    {
        builder
            .HasQueryFilter(filter: g => g.ApplicationUser.IsActive);

        builder
            .HasMany(navigationExpression: g => g.Students)
            .WithOne(navigationExpression: s => s.Guardian)
            .HasForeignKey(foreignKeyExpression: s => s.GuardianId)
            .OnDelete(DeleteBehavior.Restrict);

        SeedGuardians(builder);
    }

    private static void SeedGuardians(EntityTypeBuilder<Guardian> builder)
    {
        builder.HasData(
        new Guardian { Id = Guardian1Id }
        );
    }
}