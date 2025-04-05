using KyberKlass.Data.Models;
using KyberKlass.Web.ViewModels.Admin.User;

namespace KyberKlass.Services.Data.Interfaces.Users
{
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
    }
}