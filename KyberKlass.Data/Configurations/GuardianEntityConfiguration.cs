using KyberKlass.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KyberKlass.Data.Configurations;
public class GuardianEntityConfiguration : IEntityTypeConfiguration<Guardian>
{
    public void Configure(EntityTypeBuilder<Guardian> builder)
    {
        builder
            .HasQueryFilter(g => g.ApplicationUser.IsActive);

        builder
            .HasMany(g => g.Students)
            .WithOne(s => s.Guardian)
            .HasForeignKey(s => s.GuardianId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}