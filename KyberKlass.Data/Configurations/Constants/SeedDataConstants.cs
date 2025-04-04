namespace KyberKlass.Data.Configurations.Constants;

public static class SeedDataConstants
{
    public static class Admin
    {
        public static readonly Guid DefaultAdminUserId = new("e321fa43-9c90-4e01-8f0a-002eae899e98");
        public const string DEFAULT_ADMIN_EMAIL = "admin@kyberklass.com";
        public const string DEFAULT_ADMIN_PASSWORD = "admin";
        public static readonly Guid AdminRoleId = new("420abb62-30a5-4983-835e-fe0a46b6f463");
        public const string DEFAULT_ADMIN_ROLE_NAME = "Admin";
        public const string DEFAULT_ADMIN_FIRST_NAME = "Admin";
        public const string DEFAULT_ADMIN_LAST_NAME = "User";
        public const string DEFAULT_ADMIN_BIRTH_DATE = "01/01/2001";
        public const string DEFAULT_ADMIN_ADDRESS = "1416 Ryan Mountains";
        public const string DEFAULT_ADMIN_PUBLIC_STAMP = "0DB9D047-3375-4739-9C32-217CC8337032";
        public const string DEFAULT_ADMIN_CONCURRENCYSTAMP = "d4f6406d-9b51-4290-994d-cf1bb9668b5e";
        public const string DEFAULT_ADMIN_PHONE_NUMBER = "08888888888";

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