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

public class IdentityUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("AspNetUsers");

        SeedAdminUser(builder);
        SeedTeacherAUser(builder);
        SeedTeacherBUser(builder);
        SeedGuardianUser(builder);
        SeedStudent1User(builder);
        SeedStudent2User(builder);
    }

    private static void SeedAdminUser(EntityTypeBuilder<ApplicationUser> builder)
    {
        PasswordHasher<ApplicationUser> passwordHasher = new();
        var hashedPassword = passwordHasher.HashPassword(null!, DEFAULT_ADMIN_PASSWORD);

        builder.HasData(new ApplicationUser
        {
            Id = DefaultAdminUserId,
            UserName = DEFAULT_ADMIN_EMAIL,
            NormalizedUserName = DEFAULT_ADMIN_EMAIL.ToUpper(),
            Email = DEFAULT_ADMIN_EMAIL,
            NormalizedEmail = DEFAULT_ADMIN_EMAIL.ToUpper(),
            EmailConfirmed = false,
            FirstName = DEFAULT_ADMIN_FIRST_NAME,
            LastName = DEFAULT_ADMIN_LAST_NAME,
            BirthDate = DateTime.Parse(DEFAULT_ADMIN_BIRTH_DATE),
            Address = DEFAULT_ADMIN_ADDRESS,
            RoleId = AdminRoleId,
            PasswordHash = hashedPassword,
            SecurityStamp = DEFAULT_ADMIN_SECURITY_STAMP,
            ConcurrencyStamp = DEFAULT_ADMIN_CONCURRENCY_STAMP,
            PhoneNumber = DEFAULT_ADMIN_PHONE_NUMBER,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnd = null,
            LockoutEnabled = false,
            AccessFailedCount = 0
        });
    }

    private static void SeedTeacherAUser(EntityTypeBuilder<ApplicationUser> builder)
    {
        PasswordHasher<ApplicationUser> passwordHasher = new();
        var hashedPassword = passwordHasher.HashPassword(null!, TEACHER_PASSWORD);

        builder.HasData(new ApplicationUser
        {
            Id = TeacherAId,
            UserName = TEACHER_A_EMAIL,
            NormalizedUserName = TEACHER_A_EMAIL.ToUpper(),
            Email = TEACHER_A_EMAIL,
            NormalizedEmail = TEACHER_A_EMAIL.ToUpper(),
            EmailConfirmed = false,
            FirstName = TEACHER_A_FIRST_NAME,
            LastName = TEACHER_A_LAST_NAME,
            BirthDate = DateTime.Parse(TEACHER_A_BIRTH_DATE),
            Address = TEACHER_A_ADDRESS,
            RoleId = TeacherRoleId,
            PasswordHash = hashedPassword,
            SecurityStamp = TEACHER_A_SECURITY_STAMP,
            ConcurrencyStamp = TEACHER_A_CONCURRENCY_STAMP,
            PhoneNumber = TEACHER_A_PHONE_NUMBER,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnabled = false,
            AccessFailedCount = 0
        });
    }

    private static void SeedTeacherBUser(EntityTypeBuilder<ApplicationUser> builder)
    {
        PasswordHasher<ApplicationUser> passwordHasher = new();
        var hashedPassword = passwordHasher.HashPassword(null!, TEACHER_PASSWORD);

        builder.HasData(new ApplicationUser
        {
            Id = TeacherBId,
            UserName = TEACHER_B_EMAIL,
            NormalizedUserName = TEACHER_B_EMAIL.ToUpper(),
            Email = TEACHER_B_EMAIL,
            NormalizedEmail = TEACHER_B_EMAIL.ToUpper(),
            EmailConfirmed = false,
            FirstName = TEACHER_B_FIRST_NAME,
            LastName = TEACHER_B_LAST_NAME,
            BirthDate = DateTime.Parse(TEACHER_B_BIRTH_DATE),
            Address = TEACHER_B_ADDRESS,
            RoleId = TeacherRoleId,
            PasswordHash = hashedPassword,
            SecurityStamp = TEACHER_B_SECURITY_STAMP,
            ConcurrencyStamp = TEACHER_B_CONCURRENCY_STAMP,
            PhoneNumber = TEACHER_B_PHONE_NUMBER,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnabled = false,
            AccessFailedCount = 0
        });
    }

    private static void SeedGuardianUser(EntityTypeBuilder<ApplicationUser> builder)
    {
        PasswordHasher<ApplicationUser> passwordHasher = new();
        var hashedPassword = passwordHasher.HashPassword(null!, GUARDIAN_PASSWORD);

        builder.HasData(new ApplicationUser
        {
            Id = Guardian1Id,
            UserName = GUARDIAN_1_EMAIL,
            NormalizedUserName = GUARDIAN_1_EMAIL.ToUpper(),
            Email = GUARDIAN_1_EMAIL,
            NormalizedEmail = GUARDIAN_1_EMAIL.ToUpper(),
            EmailConfirmed = false,
            FirstName = GUARDIAN_1_FIRST_NAME,
            LastName = GUARDIAN_1_LAST_NAME,
            BirthDate = DateTime.Parse(GUARDIAN_1_BIRTH_DATE),
            Address = GUARDIAN_1_ADDRESS,
            RoleId = GuardianRoleId,
            PasswordHash = hashedPassword,
            SecurityStamp = GUARDIAN_1_SECURITY_STAMP,
            ConcurrencyStamp = GUARDIAN_1_CONCURRENCY_STAMP,
            PhoneNumber = GUARDIAN_1_PHONE_NUMBER,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnabled = false,
            AccessFailedCount = 0
        });
    }

    private static void SeedStudent1User(EntityTypeBuilder<ApplicationUser> builder)
    {
        PasswordHasher<ApplicationUser> passwordHasher = new();
        var hashedPassword = passwordHasher.HashPassword(null!, STUDENT_PASSWORD);
     
        builder.HasData(new ApplicationUser
        {
            Id = Student1Id,
            UserName = STUDENT_1_EMAIL,
            NormalizedUserName = STUDENT_1_EMAIL.ToUpper(),
            Email = STUDENT_1_EMAIL,
            NormalizedEmail = STUDENT_1_EMAIL.ToUpper(),
            EmailConfirmed = false,
            FirstName = STUDENT_1_FIRST_NAME,
            LastName = STUDENT_1_LAST_NAME,
            BirthDate = DateTime.Parse("2010-09-01"),
            Address = GUARDIAN_1_ADDRESS,
            RoleId = StudentRoleId,
            PasswordHash = hashedPassword,
            SecurityStamp = STUDENT_1_SECURITY_STAMP,
            ConcurrencyStamp = STUDENT_1_CONCURRENCY_STAMP,
            PhoneNumber = STUDENT_1_PHONE_NUMBER,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnabled = false,
            AccessFailedCount = 0
        });
    }

    private static void SeedStudent2User(EntityTypeBuilder<ApplicationUser> builder)
    {
        PasswordHasher<ApplicationUser> passwordHasher = new();
        var hashedPassword = passwordHasher.HashPassword(null!, STUDENT_2_PASSWORD);
        
        builder.HasData(new ApplicationUser
        {
            Id = Student2Id,
            UserName = STUDENT_2_EMAIL,
            NormalizedUserName = STUDENT_2_EMAIL.ToUpper(),
            Email = STUDENT_2_EMAIL,
            NormalizedEmail = STUDENT_2_EMAIL.ToUpper(),
            EmailConfirmed = false,
            FirstName = STUDENT_2_FIRST_NAME,
            LastName = STUDENT_2_LAST_NAME,
            BirthDate = DateTime.Parse("2011-03-14"),
            Address = GUARDIAN_1_ADDRESS,
            RoleId = StudentRoleId,
            PasswordHash = hashedPassword,
            SecurityStamp = STUDENT_2_SECURITY_STAMP,
            ConcurrencyStamp = STUDENT_2_CONCURRENCY_STAMP,
            PhoneNumber = STUDENT_2_PHONE_NUMBER,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnabled = false,
            AccessFailedCount = 0
        });
    }
}