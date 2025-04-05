using KyberKlass.Data.Models;
using KyberKlass.Web.ViewModels.Admin;

namespace KyberKlass.Services.Data.Interfaces.Guardians;
/// <summary>
///     Interface defining operations for retrieving guardian information.
/// </summary>
public interface IGuardianService
{
    /// <summary>
    ///     Retrieves a guardian entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="userId">The unique identifier of the guardian.</param>
    /// <returns>The guardian entity if found; otherwise, null.</returns>
    Task<Guardian?> GetByIdAsync(string userId);

    /// <summary>
    ///     Retrieves a collection of basic view models for all guardians asynchronously.
    /// </summary>
    /// <returns>A collection of basic view models representing all guardians.</returns>
    Task<IEnumerable<BasicViewModel>> GetAllGuardiansAsync();

    /// <summary>
    ///     Checks if a guardian is assigned to any student asynchronously.
    /// </summary>
    /// <param name="userId">The unique identifier of the guardian.</param>
    /// <returns>True if the guardian is assigned to any student; otherwise, false.</returns>
    Task<bool> IsGuardianAssignedToStudentAsync(string userId);

    /// <summary>
    ///     Retrieves the guardian assigned to a student asynchronously.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    Task<Guardian?> GetGuardianAssignedByUserIdAsync(string userId);

    /// <summary>
    ///     Retrieves a collection of basic view models representing students assigned to a guardian asynchronously.
    /// </summary>
    /// <param name="guardian">The guardian.</param>
    /// <returns>A collection of basic view models representing students assigned to the guardian.</returns>
    Task<IEnumerable<BasicViewModel>> GetStudentsAssignedToGuardianAsync(Guardian guardian);
}