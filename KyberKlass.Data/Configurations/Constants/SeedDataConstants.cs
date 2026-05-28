namespace KyberKlass.Data.Configurations.Constants;

public static class SeedDataConstants
{
    public static class Admin
    {
        public const string DEFAULT_ADMIN_EMAIL = "admin@kyberklass.com";
        public const string DEFAULT_ADMIN_PASSWORD = "admin";
        public const string DEFAULT_ADMIN_FIRST_NAME = "Admin";
        public const string DEFAULT_ADMIN_LAST_NAME = "User";
        public const string DEFAULT_ADMIN_BIRTH_DATE = "01/01/2001";
        public const string DEFAULT_ADMIN_ADDRESS = "1416 Ryan Mountains";
        public const string DEFAULT_ADMIN_SECURITY_STAMP = "0DB9D047-3375-4739-9C32-217CC8337032";
        public const string DEFAULT_ADMIN_CONCURRENCY_STAMP = "d4f6406d-9b51-4290-994d-cf1bb9668b5e";
        public const string DEFAULT_ADMIN_PHONE_NUMBER = "08888888888";
        public static readonly Guid DefaultAdminUserId = new("e321fa43-9c90-4e01-8f0a-002eae899e98");
        public static readonly Guid AdminRoleId = new("420abb62-30a5-4983-835e-fe0a46b6f463");
    }

    public static class Teacher
    {
        public const string TEACHER_PASSWORD = "teacher";

        public const string TEACHER_A_EMAIL = "teacher@kyberklass.com";
        public const string TEACHER_A_FIRST_NAME = "Alice";
        public const string TEACHER_A_LAST_NAME = "Johnson";
        public const string TEACHER_A_BIRTH_DATE = "08/08/1999";
        public const string TEACHER_A_ADDRESS = "15 Teacher Lane";
        public const string TEACHER_A_SECURITY_STAMP = "A1B2C3D4-E5F6-7890-ABCD-EF1234567890";
        public const string TEACHER_A_CONCURRENCY_STAMP = "0f9d3f0d-9f65-4f72-9a3f-3f95cb98f8f1";
        public const string TEACHER_A_PHONE_NUMBER = "0881111111";
        public const string TEACHER_B_EMAIL = "teacher2@kyberklass.com";
        public const string TEACHER_B_FIRST_NAME = "Leon";
        public const string TEACHER_B_LAST_NAME = "Leblanc";
        public const string TEACHER_B_BIRTH_DATE = "09/09/1998";
        public const string TEACHER_B_ADDRESS = "16 Teacher Blvd";
        public const string TEACHER_B_SECURITY_STAMP = "B2C3D4E5-F6A7-8901-BCDE-F12345678901";
        public const string TEACHER_B_CONCURRENCY_STAMP = "2c6e4b80-1b6a-4f1d-a83d-4bfbda34f4c7";
        public const string TEACHER_B_PHONE_NUMBER = "0882222222";
        public static readonly Guid TeacherAId = new("427a6f9a-bee1-4493-9d13-65ab478bb5f5");

        public static readonly Guid TeacherBId = new("9dae1d79-f917-4acd-bc32-b78408e702a3");
    }

    public static class School
    {
        public static readonly Guid School1Id = new("6cead59f-b8d4-4528-bd6d-e0e5dc97eaee");
        public static readonly Guid School2Id = new("08ed558b-d9ee-4663-b2b7-e700fd3620e6");
        public static readonly Guid School3Id = new("1e633a7a-7d6c-413a-b8bf-3081324cee7e");
    }

    public static class Role
    {
        public static readonly Guid TeacherRoleId = new("3f262f2b-bd05-4b24-9a6c-bca365d372fb");
        public static readonly Guid StudentRoleId = new("f697e34c-843e-467a-b11f-037e185c180f");
        public static readonly Guid GuardianRoleId = new("74efc216-b31e-4ed3-96d0-7dc2296eec3b");
    }

    public static class Classroom
    {
        public const string CLASSROOM_A_NAME = "A";
        public const string CLASSROOM_B_NAME = "B";
        public static readonly Guid ClassroomAId = new("3b9a4da2-8b83-4e71-ab43-916663ca9ca4");

        public static readonly Guid ClassroomBId = new("63e35421-bd57-4de6-940d-e239c1886ad5");
    }

    public static class Guardian
    {
        public const string GUARDIAN_PASSWORD = "guardian";

        // Guardian 1
        public const string GUARDIAN_1_EMAIL = "guardian@kyberklass.com";
        public const string GUARDIAN_1_FIRST_NAME = "Belle";
        public const string GUARDIAN_1_LAST_NAME = "Moon";
        public const string GUARDIAN_1_BIRTH_DATE = "02/02/2002";
        public const string GUARDIAN_1_ADDRESS = "4445 Smithfield Avenue";
        public const string GUARDIAN_1_SECURITY_STAMP = "0DB9D047-3375-4739-9C32-217CC8337032";
        public const string GUARDIAN_1_CONCURRENCY_STAMP = "d4f6406d-9b51-4290-994d-cf1bb9668b5e";
        public const string GUARDIAN_1_PHONE_NUMBER = "08844444444";
        public static readonly Guid Guardian1Id = new("db0dc8be-db06-4b53-a0e3-1a5cb3fa40da");
    }

    public static class Student
    {
        public const string STUDENT_PASSWORD = "student";

        // Student 1
        public const string STUDENT_1_EMAIL = "student@kyberklass.com";
        public const string STUDENT_1_FIRST_NAME = "Neo";
        public const string STUDENT_1_LAST_NAME = "Joyce";
        public const string STUDENT_1_PHONE_NUMBER = "0881234567";
        public const string STUDENT_1_SECURITY_STAMP = "A7D2F941-5C88-4E19-B3D0-6F2A8C7E1B44";
        public const string STUDENT_1_CONCURRENCY_STAMP = "4f8c1d7a-9b22-4d66-a5e1-2c7f9b0d3e55";
        public const string STUDENT_2_EMAIL = "student2@kyberklass.com";
        public const string STUDENT_2_PASSWORD = "student123";
        public const string STUDENT_2_FIRST_NAME = "Crystal";
        public const string STUDENT_2_LAST_NAME = "Lane";
        public const string STUDENT_2_PHONE_NUMBER = "0887654321";
        public const string STUDENT_2_SECURITY_STAMP = "3E9B1C57-8A44-42F0-9D61-5C7A2E8F0B19";
        public const string STUDENT_2_CONCURRENCY_STAMP = "b7d2e4f9-1c88-4a10-93f6-7e2c5d1a9b33";
        public static readonly Guid Student1Id = new("b0aab2ce-41d6-4a7e-b9d4-0b2c7ced288a");

        // Student 2
        public static readonly Guid Student2Id = new("58d10548-8f58-48ba-9eff-73e0d87bf7b9");
    }
}