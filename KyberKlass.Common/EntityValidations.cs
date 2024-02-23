namespace KyberKlass.Common;

public static class EntityValidations
{
	public static class Person
	{
		public const int MIN_NAME_LENGTH = 2;
		public const int MAX_NAME_LENGTH = 30;
	}

	public static class Student
	{
		public const int MIN_GRADE_LEVEL = 1;
		public const int MAX_GRADE_LEVEL = 12;

		public const double MIN_GRADE_MARK = 2.00;
		public const double MAX_GRADE_MARK = 6.00;
	}
}