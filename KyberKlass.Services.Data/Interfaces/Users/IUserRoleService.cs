using KyberKlass.Web.ViewModels.Admin.User;

namespace KyberKlass.Services.Data.Interfaces.Users
{
    public interface IUserRoleService
    {
        /// <summary>
        ///     Retrieves all available user roles asynchronously.
        /// </summary>
        /// <returns>A collection of user roles.</returns>
        Task<IEnumerable<UserRolesViewModel>> GetAllRolesAsync();

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
    }
}
