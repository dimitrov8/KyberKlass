namespace KyberKlass.Common;

public static class CustomMessageConstants
{
	public static class Common
	{
		public const string INVALID_INPUT_MESSAGE = "Invalid input. Please check your data and try again.";

		public const string UNABLE_TO_SAVE_CHANGES_MESSAGE = "Something went wrong trying to save the changes you made. Please try again.";
	}


	public static class School
	{
		public const string ALREADY_ADDED_MESSAGE = "School \"{0}\" is already added.";
		
		public const string SUCCESSFULLY_ADDED_MESSAGE = "Successfully added School \"{0}\".";

		public const string ADDITION_ERROR_MESSAGE = "An error occurred while adding the school.";

		public const string SUCCESSFULLY_APPLIED_CHANGES = "Successfully applied changes for School: \"{0}\".";

		public const string EDIT_ERROR_MESSAGE = "An error occurred while editing the school.";

		public const string SUCCESSFULLY_DELETED_MESSAGE = "School successfully deleted.";
		
		public const string SUCCESSFULLY_SOFT_DELETED_MESSAGE = "Successfully soft deleted School : \"{0}\".";

		public const string DELETION_ERROR_MESSAGE = "Failed to delete the school. Please try again later.";

	}
}