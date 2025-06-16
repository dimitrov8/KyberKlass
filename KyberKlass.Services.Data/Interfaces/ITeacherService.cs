using KyberKlass.Web.ViewModels.Admin;
using KyberKlass.Web.ViewModels.Admin.User;

namespace KyberKlass.Services.Data.Interfaces;
/// <summary>
///     Interface for managing teacher-related operations.
/// </summary>
public interface ITeacherService
{
    /// <summary>
    ///     Retrieves  a collection of teacher view models asynchronously, optionally filtered by a search term.
    /// </summary>
    ///   /// <param name="searchTerm">Optional search term to filter users.</param>
    /// <returns>A collection of user view models.</returns>
    Task<IEnumerable<UserViewModel>> AllAsync(string? searchTerm = null);

    Task<IEnumerable<BasicViewModel>> GetUnassignedTeachersAsync();

    /// <summary>
    ///     Checks if a teacher is assigned to any classroom asynchronously.
    /// </summary>
    /// <param name="userId">The unique identifier of the teacher.</param>
    /// <returns>True if the teacher is assigned to any classroom; otherwise, false.</returns>
    Task<bool> IsTeacherAssignedToClassroomAsync(string userId);
}