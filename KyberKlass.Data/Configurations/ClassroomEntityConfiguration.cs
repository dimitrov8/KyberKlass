using KyberKlass.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static KyberKlass.Data.Configurations.Constants.SeedDataConstants.School;
using static KyberKlass.Data.Configurations.Constants.SeedDataConstants.Teacher;
using static KyberKlass.Data.Configurations.Constants.SeedDataConstants.Classroom;

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
        
        SeedClassrooms(builder);
    }
    
    private static void SeedClassrooms(EntityTypeBuilder<Classroom> builder)
    {
        builder.HasData(
            new Classroom
            {
                Id = ClassroomAId,
                Name = CLASSROOM_A_NAME,
                TeacherId = TeacherAId,
                SchoolId = School1Id,
                IsActive = true
            },
            new Classroom
            {
                Id = ClassroomBId,
                Name = CLASSROOM_B_NAME,
                TeacherId = TeacherBId,
                SchoolId = School2Id,
                IsActive = true
            }
        );
    }
}