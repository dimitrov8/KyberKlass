namespace KyberKlass.Data.Configurations.Constants;

public static class SeedDataConstants
{
    public static class Admin
    {
        public static readonly Guid DefaultAdminUserId = new("e321fa43-9c90-4e01-8f0a-002eae899e98");
        public const string DEFAULT_ADMIN_EMAIL = "admin@kyberklass.com";
        public static readonly Guid AdminRoleId = new("420abb62-30a5-4983-835e-fe0a46b6f463");
    }

    public static class Teacher
    {
        public static readonly Guid DefaultTeacherUserId = new("63019936-f684-4447-bcda-51393418093f");

        public static readonly Guid TeacherAId = new("427a6f9a-bee1-4493-9d13-65ab478bb5f5");
        public const string TEACHER_A_EMAIL = "teacher@kyberklass.com";

        public static readonly Guid TeacherBId = new("9dae1d79-f917-4acd-bc32-b78408e702a3");
        public const string TEACHER_B_EMAIL = "teacher2@kyberklass.com";
    }
}