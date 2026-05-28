using KyberKlass.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static KyberKlass.Data.Configurations.Constants.SeedDataConstants.Admin;
using static KyberKlass.Data.Configurations.Constants.SeedDataConstants.Teacher;
using static KyberKlass.Data.Configurations.Constants.SeedDataConstants.Role;
using static KyberKlass.Data.Configurations.Constants.SeedDataConstants.Guardian;
using static KyberKlass.Data.Configurations.Constants.SeedDataConstants.Student;

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
            // Admin
            builder.HasData(new IdentityUserRole<Guid>
            {
                UserId = DefaultAdminUserId,
                RoleId = AdminRoleId 
            });

            // Teacher A
            builder.HasData(new IdentityUserRole<Guid>
            {
                UserId = TeacherAId, 
                RoleId = TeacherRoleId
            });
            
            // Teacher B
            builder.HasData(new IdentityUserRole<Guid>
            {
                UserId = TeacherBId, 
                RoleId = TeacherRoleId
            }); 
            
            // Guardian 1
            builder.HasData(new IdentityUserRole<Guid>
            {
                UserId = Guardian1Id, 
                RoleId = GuardianRoleId
            });
            
            // Student 1
            builder.HasData(new IdentityUserRole<Guid>
            {
                UserId = Student1Id, 
                RoleId = StudentRoleId
            });
            
            builder.HasData(new IdentityUserRole<Guid>
            {
                UserId = Student2Id, 
                RoleId = StudentRoleId
            });
        }
        catch (Exception ex)
        {
            // Output error message to console
            Console.WriteLine($"Error occurred while seeding admin user role: {ex.Message}");
        }
    }
}