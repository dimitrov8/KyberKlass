namespace KyberKlass.Services.Data;

using System.Globalization;
using Interfaces;
using KyberKlass.Data;
using KyberKlass.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.Admin.User;

public class UserService : IUserService
{
	private readonly KyberKlassDbContext _dbContext;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly RoleManager<IdentityRole<Guid>> _roleManager;

	public UserService(KyberKlassDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
	{
		this._dbContext = dbContext;
		this._userManager = userManager;
		this._roleManager = roleManager;
	}

	public async Task<List<UserViewModel>> AllAsync()
	{
		var usersWithRoles = await this._dbContext.Users
			.Select(user => new
			{
				User = user,
				Roles = this._dbContext.UserRoles
					.Where(ur => ur.UserId == user.Id)
					.Join(this._dbContext.Roles,
						ur => ur.RoleId,
						role => role.Id,
						(ur, role) => role.Name)
					.ToList()
			})
			.ToListAsync();

		List<UserViewModel> userViewModels = usersWithRoles
			.Select(u => new UserViewModel
			{
				Id = u.User.Id.ToString(),
				FullName = $"{u.User.FirstName} {u.User.LastName}",
				Email = u.User.Email,
				Role = u.Roles.FirstOrDefault() ?? "No Role Assigned",
				IsActive = u.User.IsActive
			})
			.ToList();

		return userViewModels;
	}

	public async Task<UserDetailsViewModel?> GetUserDetailsAsync(string id)
	{
		var user = await this._dbContext
			.Users.FirstOrDefaultAsync(u => u.Id.ToString() == id);

		if (user == null)
		{
			return null;
		}

		IList<string>? userRoles = await this._userManager.GetRolesAsync(user);
		string currentUserRole = userRoles.FirstOrDefault() ?? "No Role Assigned";

		var viewModel = new UserDetailsViewModel
		{
			Id = user.Id.ToString(),
			FullName = $"{user.FirstName} {user.LastName}",
			BirthDate = user.BirthDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
			Address = user.Address,
			PhoneNumber = user.PhoneNumber,
			Email = user.Email,
			Role = currentUserRole,
			IsActive = user.IsActive ? "True" : "False"
		};

		return viewModel;
	}

	public async Task<UserUpdateRoleViewModel?> GetUserForUpdateRoleAsync(string id)
	{
		var user = await this._dbContext
			.Users
			.FirstOrDefaultAsync(u => u.Id.ToString() == id);

		if (user == null)
		{
			return null;
		}

		IList<string>? userRoles = await this._userManager.GetRolesAsync(user);
		string currentUserRole = userRoles.FirstOrDefault() ?? "No Role Assigned";

		IEnumerable<UserRolesViewModel> availableRoles = await this.GetAllRolesAsync();

		var viewModel = new UserUpdateRoleViewModel
		{
			Id = user.Id.ToString(),
			FullName = $"{user.FirstName} {user.LastName}",
			Email = user.Email,
			IsActive = user.IsActive,
			CurrentRoleName = currentUserRole,
			AvailableRoles = availableRoles
		};

		return viewModel;
	}

	public async Task<bool> IsNotNullOrEmptyInputAsync(string id, UserViewModel? model)
	{
		if (string.IsNullOrEmpty(id))
		{
			return false;
		}

		if (model != null && string.IsNullOrEmpty(model.Id))
		{
			return false;
		}

		return true;
	}

	public async Task<bool> UpdateRoleAsync(string id, string roleId)
	{
		var user = await this._dbContext
			.Users
			.FirstOrDefaultAsync(u => u.Id.ToString() == id);

		if (user == null)
		{
			return false;
		}


		IdentityRole<Guid>? role = await this._roleManager
			.Roles
			.FirstOrDefaultAsync(r => r.Id.ToString() == roleId);

		if (role == null)
		{
			return false;
		}

		// Check if the user already has the selected role
		IList<string>? userRoles = await this._userManager.GetRolesAsync(user);

		if (userRoles.Contains(role.Name))
		{
			return true; // User has the selected role, no need to override it
		}

		// Else if the user is being assigned different role to it's current 
		foreach (string? userRole in userRoles)
		{
			var result = await this._userManager.RemoveFromRoleAsync(user, userRole); // Remove his current role

			if (result.Succeeded == false) // If something happened and the role was unable to be removed from the user
			{
				return false; // Return false
			}
		}

		// Tries to add the new role to the user
		var addResult = await this._userManager.AddToRoleAsync(user, role.Name);

		return addResult.Succeeded; // Returns true if the role was successfully assigned to the user, else returns false
	}

	public async Task<IEnumerable<UserRolesViewModel>> GetAllRolesAsync() // Gets all roles and returns them as IEnumerable<UserRolesViewModel>
	{
		IEnumerable<UserRolesViewModel> allRoles = await this._roleManager
			.Roles
			.Select(r => new UserRolesViewModel
			{
				Id = r.Id.ToString(),
				Name = r.Name
			})
			.AsNoTracking()
			.ToArrayAsync();

		return allRoles;
	}
}