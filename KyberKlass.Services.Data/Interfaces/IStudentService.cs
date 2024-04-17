namespace KyberKlass.Services.Data.Interfaces;

using KyberKlass.Data.Models;
using Web.ViewModels.Admin.Student;
using Web.ViewModels.Admin.User;

/// <summary>
///     Interface for managing student-related operations.
/// </summary>
public interface IStudentService
{
    /// <summary>
    /// Retrieves all users asynchronously.
    /// </summary>
    /// <returns>A collection of UserViewModels or null if none found.</returns>
    Task<IEnumerable<UserViewModel>?> AllAsync();

    /// <summary>
    /// Retrieves a student by their unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the student.</param>
    /// <returns>The Student object if found, otherwise null.</returns>
    Task<Student?> GetByIdASync(string id);

    /// <summary>
    /// Retrieves the view model for changing a student's guardian asynchronously.
    /// </summary>
    /// <param name="userId">The unique identifier of the student</param>
    /// <returns>The StudentChangeGuardianViewModel for the specified student.</returns>
    Task<StudentChangeGuardianViewModel> GetStudentChangeGuardianAsync(string userId);

    /// <summary>
    /// Changes the guardian of a student asynchronously.
    /// </summary>
    /// <param name="userId">The unique identifier of the student.</param>
    /// <param name="guardianId">The unique identifier of the new guardian.</param>
    /// <returns>True if the guardian was changed successfully, otherwise false.</returns>
    Task<bool> StudentChangeGuardianAsync(string userId, string guardianId);
}