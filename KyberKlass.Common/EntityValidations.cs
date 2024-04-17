namespace KyberKlass.Common;

public static class EntityValidations
{
    public static class BaseUser
    {
        public const int MIN_NAME_LENGTH = 2;
        public const int MAX_NAME_LENGTH = 30;

        public const int MIN_ADDRESS_LENGTH = 5;
        public const int MAX_ADDRESS_LENGTH = 100;
    }

    public static class School
    {
        public const int MIN_NAME_LENGTH = 4;
        public const int MAX_NAME_LENGTH = 100;

        public const int MIN_ADDRESS_LENGTH = 5;
        public const int MAX_ADDRESS_LENGTH = 100;
    }

    public static class Classroom
    {
        public const int MIN_NAME_LENGTH = 1;
        public const int MAX_NAME_LENGTH = 5;
    }
}