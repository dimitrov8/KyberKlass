#region

using KyberKlass.Data.Models;
using KyberKlass.Web.ViewModels.Admin.Student;
using KyberKlass.Web.ViewModels.Admin.User;

#endregion

namespace KyberKlass.Services.Data.Interfaces;

/// <summary>
///     Interface for managing student-related operations.
/// </summary>
public interface IStudentService
{
    /// <summary>
    ///     Retrieves all users asynchronously, optionally filtered by a search term.
    /// </summary>
    /// <param name="searchTerm">Optional search term to filter schools.</param>
    /// <returns>A collection of user view models.</returns>
    Task<IEnumerable<UserViewModel>> AllAsync(string? searchTerm = null);

    /// <summary>
    ///     Retrieves a student by their unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the student.</param>
    /// <returns>The Student object if found, otherwise null.</returns>
    Task<Student?> GetByIdAsync(string id);

    /// <summary>
    ///     Retrieves the view model for changing a student's guardian asynchronously.
    /// </summary>
    /// <param name="userId">The unique identifier of the student</param>
    /// <returns>The StudentChangeGuardianViewModel for the specified student.</returns>
    Task<StudentChangeGuardianViewModel> GetStudentChangeGuardianAsync(string userId);

    /// <summary>
    ///     Changes the guardian of a student asynchronously.
    /// </summary>
    /// <param name="userId">The unique identifier of the student.</param>
    /// <param name="guardianId">The unique identifier of the new guardian.</param>
    /// <returns>True if the guardian was changed successfully, otherwise false.</returns>
    Task<bool> StudentChangeGuardianAsync(string userId, string guardianId);
}