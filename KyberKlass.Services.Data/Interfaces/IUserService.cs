namespace KyberKlass.Services.Data.Interfaces;

using Web.ViewModels.Admin.User;

public interface IUserService
{
	Task<List<UserViewModel>> AllAsync();

	Task<UserDetailsViewModel?> GetUserDetailsAsync(string id);

	Task<UserUpdateRoleViewModel?> GetUserForUpdateRoleAsync(string id);

	Task<bool> IsNotNullOrEmptyInputAsync(string id, UserViewModel? model);

	Task<bool> UpdateRoleAsync(string id, string roleId);

	Task<IEnumerable<UserRolesViewModel>> GetAllRolesAsync();
}