namespace KyberKlass.Services.Data.Interfaces;

using Web.ViewModels.Admin;
using Web.ViewModels.Admin.User;

public interface ITeacherService
{
	Task<List<UserViewModel>?> AllAsync();

	Task<IEnumerable<BasicViewModel>> GetUnassignedTeachersAsync();

	/// <summary>
	/// Checks if a teacher is assigned to any classroom asynchronously.
	/// </summary>
	/// <param name="userId">The unique identifier of the teacher.</param>
	/// <returns>True if the teacher is assigned to any classroom; otherwise, false.</returns>
	Task<bool> IsTeacherAssignedToClassroomAsync(string userId);
}