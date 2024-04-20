namespace KyberKlass.Services.Data.Interfaces;

using KyberKlass.Data.Models;
using Web.ViewModels.Admin;
using Web.ViewModels.Admin.User;

/// <summary>
///     Interface for managing user-related operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    ///     Retrieves a collection of user view models asynchronously.
    /// </summary>
    /// <returns>A collection of user view models.</returns>
    Task<IEnumerable<UserViewModel>> AllAsync();

    /// <summary>
    ///     Retrieves a user entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>The user entity if found; otherwise, null.</returns>
    Task<ApplicationUser?> GetUserById(string id);

    /// <summary>
    ///     Retrieves all available user roles asynchronously.
    /// </summary>
    /// <returns>A collection of user roles.</returns>
    Task<IEnumerable<UserRolesViewModel>> GetAllRolesAsync();

    /// <summary>
    ///     Retrieves details of a user by their unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>The details of the user if found; otherwise, null.</returns>
    Task<UserDetailsViewModel?> GetDetailsAsync(string id);

    /// <summary>
    ///     Retrieves a view model for editing a user asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>A view model for editing a user if the user is found; otherwise, null.</returns>
    Task<UserEditFormModel?> GetForEditAsync(string id);

    /// <summary>
    ///     Edits user information asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="model">The updated user information.</param>
    /// <returns>The updated user edit form model.</returns>
    Task<UserEditFormModel?> EditAsync(string id, UserEditFormModel model);

    /// <summary>
    ///     Retrieves a view model for updating user roles asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>A view model for updating user roles if the user is found; otherwise, null.</returns>
    Task<UserUpdateRoleViewModel?> GetForUpdateRoleAsync(string id);

    /// <summary>
    ///     Retrieves the name of the role corresponding to the given identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the role.</param>
    /// <returns>The name of the role if found; otherwise, null.</returns>
    Task<string?> GetRoleNameByIdAsync(string id);

    /// <summary>
    ///     Updates the role of a user asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="roleId">The unique identifier of the role to assign to the user.</param>
    /// <param name="guardianId">The unique identifier of the guardian (if applicable).</param>
    /// <param name="schoolId">The unique identifier of the school (if applicable).</param>
    /// <param name="classroomId">The unique identifier of the classroom (if applicable).</param>
    /// <returns>True if the role update was successful; otherwise, false.</returns>
    Task<bool> UpdateRoleAsync(string id, string roleId, string? guardianId, string? schoolId, string? classroomId);

    /// <summary>
    ///     Retrieves a collection of basic view models representing students assigned to a guardian asynchronously.
    /// </summary>
    /// <param name="guardian">The guardian.</param>
    /// <returns>A collection of basic view models representing students assigned to the guardian.</returns>
    Task<IEnumerable<BasicViewModel>> GetStudentsAssignedToGuardianAsync(Guardian guardian);
}