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

	public UserService(KyberKlassDbContext dbContext, UserManager<ApplicationUser> userManager)
	{
		this._dbContext = dbContext;
		this._userManager = userManager;
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
		ApplicationUser? user = await this._dbContext
			.Users.FirstOrDefaultAsync(u => u.Id.ToString() == id);

		if (user == null)
		{
			return null;
		}

		var  roles = await this._userManager.GetRolesAsync(user);
		string userRole = roles.FirstOrDefault() ?? "No Role Assigned";

		var viewModel = new UserDetailsViewModel
		{
			Id = user.Id.ToString(),
			FullName = $"{user.FirstName} {user.LastName}",
			BirthDate = user.BirthDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
			Address = user.Address,
			PhoneNumber = user.PhoneNumber,
			Email = user.Email,
			Role = userRole,
			IsActive = user.IsActive ? "True" : "False"
		};

		return viewModel;
	}
}