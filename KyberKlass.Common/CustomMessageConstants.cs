namespace KyberKlass.Common;

public static class CustomMessageConstants
{
	public static class Common
	{
		public const string INVALID_INPUT_MESSAGE = "Invalid input. Please check your data and try again."; // Invalid input

		// ADDITION
		public const string ADDITION_SUCCESSFUL_MESSAGE = "{0} \"{1}\" is successfully added."; // {0} => name of controller | {1} => name of model
		public const string ALREADY_ADDED_MESSAGE = "{0} \"{1}\" already exists."; // {0} => name of controller | {1} => name of model
		public const string UNABLE_TO_ADD_MESSAGE = "Unable to add {0}. Please try again."; // {0} => name of controller
		public const string ADDITION_ERROR_MESSAGE = "An error occurred while adding the {0}."; // {0} => name of controller

		// EDIT
		public const string CHANGES_SUCCESSFULLY_APPLIED_MESSAGE = "Successfully applied changes for {0} with ID: \"{1}\"."; // {0} => name of controller | {1} => name of model
		public const string UNABLE_TO_SAVE_CHANGES_MESSAGE = "Something went wrong trying to save the changes you made. Please try again."; // Unable to save changes
		public const string EDIT_ERROR_MESSAGE = "An error occurred while editing the {0}."; // {0} => name of controller

		// DELETION
		public const string DELETION_SUCCESSFUL_MESSAGE = "{0} with ID: \"{1}\" succefully deleted."; // {0} => name of controller | {1} => name of model
		public const string SOFT_DELETION_SUCCESSFUL_MESSAGE = "Successfully soft deleted {0} with ID: \"{1}\"."; // {0} => name of controller | {1} => id of model
		public const string DELETION_ERROR_MESSAGE = "{0} with ID: \"{1}\" delation failed. Please try again later."; // {0} => name of controller
	}

	public static class User
	{
		// Role
		public const string ROLE_UPDATE_SUCCESS_MESSAGE = "Successfully updated role for User with ID: \"{0}\".";
		public const string ROLE_UPDATE_FAILED_MESSAGE = "Failed to update role for User with ID: \"{0}\".";

        public const string FAILED_TO_UPDATE_TEACHER_TO_OTHER_ROLE_MESSAGE = "Unable to update role. User with ID: \"{0}\" is currently assigned as a teacher in a classroom. " +
		                                                                     "Please reassign the classroom teacher before attempting to update the role.";
		public const string FAILED_TO_UPDATE_GUARDIAN_TO_OTHER_ROLE_MESSAGE = "Unable to update role. User with ID: \"{0}\" is currently assigned as a guardian for one or more students. " +
		                                                                      "Please reassign the guardian for the affected student(s) before attempting to update the role.";

		public const string FAILED_TO_UPDATE_TO_STUDENT_ROLE_MESSAGE = "Failed to update role to Student. Invalid guardian, school, or classroom.";
	}

	public static class Student
	{
		public const string SUCCESSFULLY_CHANGED_GUARDIAN = "Successfully changed guardian for student with ID: {0}.";

		public const string FAILED_TO_CHANGE_GUARDIAN = "Failed to changed guardian for student with ID: {0}.";

		public const string GUARDIAN_ALREADY_SET = "You cannot change your guardian to the current guardian because it is already set.";
	}
}