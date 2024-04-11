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

		public const int MIN_COUNTRY_NAME_LENGTH = 4;
		public const int MAX_COUNTRY_NAME_LENGTH = 56;

		public const int MIN_CITY_VILLAGE_NAME_LENGTH = 4;
		public const int MAX_CITY_VILLAGE_NAME_LENGTH = 85;

		public const int MIN_ADDRESS_LENGTH = 5;
		public const int MAX_ADDRESS_LENGTH = 100;
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

		public const int GRADE_LENGTH = 4;
		public const string GRADE_REGEX = @"^[2-6]\.\d{2}$";

		public const string MIN_GRADE_VALUE = "2.00";
		public const string MAX_GRADE_VALUE = "6.00";
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