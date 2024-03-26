namespace KyberKlass.Common;

public static class CustomMessageConstants
{
	public static class Common
	{
		public const string INVALID_INPUT_MESSAGE = "Invalid input. Please check your data and try again.";

		public const string UNABLE_TO_SAVE_CHANGES_MESSAGE = "Something went wrong trying to save the changes you made. Please try again.";

		public const string ALREADY_ADDED_MESSAGE = "{0} \"{1}\" is already added.";

		public const string SUCCESSFULLY_ADDED_MESSAGE = "Successfully added {0} \"{1}\".";

		public const string ADDITION_ERROR_MESSAGE = "An error occurred while adding the {0}.";

		public const string EDIT_ERROR_MESSAGE = "An error occurred while editing the {0}.";
	}


	public static class School
	{
		public const string SUCCESSFULLY_APPLIED_CHANGES = "Successfully applied changes for School: \"{0}\".";

		public const string SUCCESSFULLY_DELETED_MESSAGE = "School successfully deleted.";

		public const string SUCCESSFULLY_SOFT_DELETED_MESSAGE = "Successfully soft deleted School : \"{0}\".";

		public const string DELETION_ERROR_MESSAGE = "Failed to delete the school. Please try again later.";
	}

	public static class User
	{
		public const string ROLE_UPDATE_FAILED_MESSAGE = "Failed to update user role.";

		public const string ROLE_UPDATE_SUCCESS_MESSAGE = "Successfully updated role.";

		public const string SUCCESSFULLY_APPLIED_CHANGES_MESSAGE = "Successfully applied changes for User with Id: \"{0}\".";

		public const string SUCCESSFULLY_SOFT_DELETED_MESSAGE = "Successfully soft deleted User with Id: \"{0}\".";

        public const string FAILED_TO_UPDATE_TEACHER_TO_OTHER_ROLE_MESSAGE = "Cannot update role. User is assigned as a teacher in a classroom. " +
                                                                             "Please change the teacher in the affected classroom before trying to update role.";		

    }
}