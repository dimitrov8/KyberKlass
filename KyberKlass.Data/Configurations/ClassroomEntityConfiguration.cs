using KyberKlass.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KyberKlass.Data.Configurations;
public class ClassroomEntityConfiguration : IEntityTypeConfiguration<Classroom>
{
    public void Configure(EntityTypeBuilder<Classroom> builder)
    {
        builder
            .HasQueryFilter(c => c.IsActive);

        builder
            .HasMany(c => c.Students)
            .WithOne(s => s.Classroom)
            .HasForeignKey(s => s.ClassroomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}