using KyberKlass.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static KyberKlass.Data.Configurations.Constants.SeedDataConstants.Student;
using static KyberKlass.Data.Configurations.Constants.SeedDataConstants.Guardian;
using static KyberKlass.Data.Configurations.Constants.SeedDataConstants.Classroom;
using static KyberKlass.Data.Configurations.Constants.SeedDataConstants.School;

namespace KyberKlass.Data.Configurations;

public class StudentEntityConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder
            .HasQueryFilter(static s => s.ApplicationUser.IsActive);

        builder
            .HasOne(static s => s.Guardian)
            .WithMany(static g => g.Students)
            .HasForeignKey(static s => s.GuardianId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
          .HasOne(static s => s.School)
          .WithMany(static g => g.Students)
          .HasForeignKey(static s => s.SchoolId)
          .OnDelete(DeleteBehavior.Restrict);
        
        SeedStudents(builder);
    }
    
    private static void SeedStudents(EntityTypeBuilder<Student> builder)
    {
        builder.HasData(
            new Student
            {
                Id = Student1Id,
                GuardianId = Guardian1Id,
                SchoolId = School1Id,
                ClassroomId = ClassroomAId
            },
            new Student
            {
                Id = Student2Id,
                GuardianId = Guardian1Id,
                SchoolId = School2Id,
                ClassroomId = ClassroomBId
            }
        );
    }
}