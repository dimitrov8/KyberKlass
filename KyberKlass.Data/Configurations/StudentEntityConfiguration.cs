using KyberKlass.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KyberKlass.Data.Configurations;
public class StudentEntityConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder
            .HasQueryFilter(s => s.ApplicationUser.IsActive);

        builder
            .HasOne(s => s.Guardian)
            .WithMany(g => g.Students)
            .HasForeignKey(s => s.GuardianId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(s => s.School)
            .WithMany(g => g.Students)
            .HasForeignKey(s => s.SchoolId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}