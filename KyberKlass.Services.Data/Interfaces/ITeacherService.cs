using KyberKlass.Web.ViewModels.Admin;
using KyberKlass.Web.ViewModels.Admin.User;

namespace KyberKlass.Services.Data.Interfaces;
/// <summary>
///     Interface for managing teacher-related operations.
/// </summary>
public interface ITeacherService
{
    /// <summary>
    ///     Retrieves all teachers asynchronously.
    /// </summary>
    /// <returns>A list of UserViewModels representing teachers or null if none found.</returns>
    Task<List<UserViewModel>?> AllAsync();

    Task<IEnumerable<BasicViewModel>> GetUnassignedTeachersAsync();

    /// <summary>
    ///     Checks if a teacher is assigned to any classroom asynchronously.
    /// </summary>
    /// <param name="userId">The unique identifier of the teacher.</param>
    /// <returns>True if the teacher is assigned to any classroom; otherwise, false.</returns>
    Task<bool> IsTeacherAssignedToClassroomAsync(string userId);
}