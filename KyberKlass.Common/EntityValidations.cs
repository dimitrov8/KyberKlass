namespace KyberKlass.Common;

public static class EntityValidations
{
	public static class ApplicationUser
	{
		public const int MIN_NAME_LENGTH = 2;
		public const int MAX_NAME_LENGTH = 30;
	}

	public static class School
	{
		public const int MIN_NAME_LENGTH = 4;
		public const int MAX_NAME_LENGTH = 30;
	}

	public static class Student
	{
		public const int MIN_GRADE_LEVEL = 1;
		public const int MAX_GRADE_LEVEL = 12;

		public const double MIN_GRADE_MARK = 2.00;
		public const double MAX_GRADE_MARK = 6.00;
	}

	public static class Classroom
	{
		public const int MIN_NAME_LENGTH = 1;
		public const int MAX_NAME_LENGTH = 5;
	}

	public static class Subject
	{
		public const int MIN_NAME_LENGTH = 3;
		public const int MAX_NAME_LENGTH = 30;
	}

	public static class Assignment
	{
		public const int MIN_TITLE_LENGTH = 4;
		public const int MAX_TITLE_LENGTH = 30;

		public const int MIN_DESCRIPTION_LENGTH = 5;
		public const int MAX_DESCRIPTION_LENGTH = 700;
	}

	public static class Exam
	{
		public const int MIN_DESCRIPTION_LENGTH = 0;
		public const int MAX_DESCRIPTION_LENGTH = 100;
	}

	public static class Behavior
	{
		public const int MIN_DESCRIPTION_LENGTH = 10;
		public const int MAX_DESCRIPTION_LENGTH = 100;
	}

	public static class Praise
	{
		public const int MIN_DESCRIPTION_LENGTH = 10;
		public const int MAX_DESCRIPTION_LENGTH = 100;
	}
}