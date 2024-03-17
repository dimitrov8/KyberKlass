﻿namespace KyberKlass.Services.Data.Interfaces;

using KyberKlass.Data.Models;
using Web.ViewModels.Admin.User;

public interface IUserService
{
	Task<List<UserViewModel>> AllAsync();

	Task<UserDetailsViewModel?> GetDetailsAsync(string id);

	Task<UserUpdateRoleViewModel?> GetForUpdateRoleAsync(string id);

	Task<string?> UpdateRoleAsync(string id, string roleId);

	Task<IEnumerable<UserRolesViewModel>> GetAllRolesAsync();

	Task<UserEditFormModel?> GetForEditAsync(string id);

	Task<UserEditFormModel?> EditAsync(string id, UserEditFormModel model);

	Task<ApplicationUser?> GetUserById(string id);
}