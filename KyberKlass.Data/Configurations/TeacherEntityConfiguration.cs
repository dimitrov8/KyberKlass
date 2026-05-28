#region

using KyberKlass.Data.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static KyberKlass.Data.Configurations.Constants.SeedDataConstants.Teacher;

#endregion

namespace KyberKlass.Data.Configurations;

public class TeacherEntityConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder
            .HasQueryFilter(filter: static t => t.ApplicationUser.IsActive);

        SeedTeachers(builder);
    }

    private void SeedTeachers(EntityTypeBuilder<Teacher> builder)
    {
        builder.HasData(
        new Teacher { Id = TeacherAId },
        new Teacher { Id = TeacherBId }
        );
    }
}