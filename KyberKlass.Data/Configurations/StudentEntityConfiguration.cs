#region

using KyberKlass.Data.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static KyberKlass.Data.Configurations.Constants.SeedDataConstants.Student;
using static KyberKlass.Data.Configurations.Constants.SeedDataConstants.Guardian;
using static KyberKlass.Data.Configurations.Constants.SeedDataConstants.Classroom;
using static KyberKlass.Data.Configurations.Constants.SeedDataConstants.School;

#endregion

namespace KyberKlass.Data.Configurations;

public class StudentEntityConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder
            .HasQueryFilter(filter: static s => s.ApplicationUser.IsActive);

        builder
            .HasOne(navigationExpression: static s => s.Guardian)
            .WithMany(navigationExpression: static g => g.Students)
            .HasForeignKey(foreignKeyExpression: static s => s.GuardianId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(navigationExpression: static s => s.School)
            .WithMany(navigationExpression: static g => g.Students)
            .HasForeignKey(foreignKeyExpression: static s => s.SchoolId)
            .OnDelete(DeleteBehavior.Restrict);

        SeedStudents(builder);
    }

    private static void SeedStudents(EntityTypeBuilder<Student> builder)
    {
        builder.HasData(
        new Student { Id = Student1Id, GuardianId = Guardian1Id, SchoolId = School1Id, ClassroomId = ClassroomAId },
        new Student { Id = Student2Id, GuardianId = Guardian1Id, SchoolId = School2Id, ClassroomId = ClassroomBId }
        );
    }
}