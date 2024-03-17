namespace KyberKlass.Services.Data;

using System.Globalization;
using Interfaces;
using KyberKlass.Data;
using KyberKlass.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.Admin.User;
using static Common.FormattingConstants;

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
				FullName = u.User.GetFullName(),
				Email = u.User.Email,
				Role = u.Roles.FirstOrDefault() ?? "No Role Assigned",
				IsActive = u.User.IsActive
			})
			.ToList();

		return userViewModels; // TODO Make this method better
	}

	/// <summary>
	///     Retrieves details of a user asynchronously based on the provided ID.
	/// </summary>
	/// <param name="id">The ID of the user to retrieve details for.</param>
	/// <returns>
	///     A user details view model representing the specified user, or null if the user is not found.
	/// </returns>
	public async Task<UserDetailsViewModel?> GetDetailsAsync(string id)
	{
		var user = await this.GetUserById(id); // Retrieve the user from the database based on the provided ID

		if (user == null)
		{
			return null; // Return null if the user is not found
		}

		// Map user details to a view model
		var viewModel = new UserDetailsViewModel
		{
			Id = user.Id.ToString(),
			FullName = user.GetFullName(),
			BirthDate = user.GetBirthDate(),
			Address = user.Address,
			PhoneNumber = user.PhoneNumber,
			Email = user.Email,
			Role = await user.GetRoleAsync(this._userManager), // Retrieve the user's role asynchronously
			IsActive = user.GetStatus()
		};

		return viewModel;
	}

	/// <summary>
	///     Retrieves a user asynchronously for updating their role based on the provided ID.
	/// </summary>
	/// <param name="id">The ID of the user to retrieve for updating their role.</param>
	/// <returns>
	///     A user update role view model representing the specified user, or null if the user is not found.
	/// </returns>
	public async Task<UserUpdateRoleViewModel?> GetForUpdateRoleAsync(string id)
	{
		var user = await this.GetUserById(id); // Retrieve the user from the database based on the provided ID

		if (user == null)
		{
			return null; // Return null if the user is not found
		}

		IEnumerable<UserRolesViewModel> availableRoles = await this.GetAllRolesAsync(); // Retrieve all available roles asynchronously
		IEnumerable<UserBasicViewModel> availableGuardians = await this.GetAllGuardiansAsync();

		// Create a view model for updating user role
		var viewModel = new UserUpdateRoleViewModel
		{
			Id = user.Id.ToString(),
			FullName = user.GetFullName(),
			Email = user.Email,
			IsActive = user.IsActive,
			CurrentRoleName = await user.GetRoleAsync(this._userManager), // Retrieve the user's current role asynchronously
			AvailableRoles = availableRoles, // Assign available roles to the view model
			AvailableGuardians = availableGuardians // Assign available guardians to the view model
		};

		return viewModel;
	}

	private async Task<IEnumerable<UserBasicViewModel>> GetAllGuardiansAsync()
	{
		IList<ApplicationUser> guardians = await this._userManager.GetUsersInRoleAsync("Guardian");

		List<UserBasicViewModel> guardianViewModels = guardians
			.Select(g => new UserBasicViewModel
			{
				Id = g.Id.ToString(),
				Name = g.GetFullName()
			})
			.ToList();

		return guardianViewModels;
	}

	private async Task UpdateUserRoleTableAsync(ApplicationUser user, string roleName)
	{
		switch (roleName)
		{
			case "Teacher":
				this._dbContext.Teachers.Add(new Teacher { Id = user.Id });
				break;
			case "Guardian":
				this._dbContext.Guardians.Add(new Guardian { Id = user.Id });
				break;
			case "Student":
				this._dbContext.Students.Add(new Student { Id = user.Id });
				break;
		}

		// Save changes to insert the user into the respective table
		await this._dbContext.SaveChangesAsync();
	}

	private async Task RemoveUserFromCurrentRoleTableAsync(ApplicationUser user, string currentRoleName)
	{
		switch (currentRoleName)
		{
			case "Teacher":
				var teacherToRemove = await this._dbContext.Teachers.FindAsync(user.Id);

				if (teacherToRemove != null)
				{
					this._dbContext.Teachers.Remove(teacherToRemove);
				}

				break;
			case "Guardian":
				var guardianToRemove = await this._dbContext.Guardians.FindAsync(user.Id);

				if (guardianToRemove != null)
				{
					this._dbContext.Guardians.Remove(guardianToRemove);
				}

				break;
			case "Student":
				var studentToRemove = await this._dbContext.Students.FindAsync(user.Id);

				if (studentToRemove != null)
				{
					this._dbContext.Students.Remove(studentToRemove);
				}

				break;
		}

		// Save changes to remove the user from the respective table
		await this._dbContext.SaveChangesAsync();
	}

	/// <summary>
	///     Updates the role of a user identified by the provided ID.
	///     If the user's current role matches the selected role, no changes are made.
	///     Otherwise, the user is removed from all current roles and added to the new role.
	///     The Role property of ApplicationUser is updated if necessary, and changes are saved to the database.
	///     Returns the name of the updated role if successful; otherwise, returns null.
	/// </summary>
	/// <param name="id">The ID of the user to update.</param>
	/// <param name="roleId">The ID of the new role.</param>
	/// <returns>The name of the updated role if successful; otherwise, null.</returns>
	public async Task<string?> UpdateRoleAsync(string id, string roleId)
	{
		var user = await this.GetUserById(id);

		if (user == null)
		{
			return null;
		}

		// Use the GetRoleAsync method from ApplicationUser to get the user's role
		IdentityRole<Guid>? role = await this._roleManager
			.Roles
			.FirstOrDefaultAsync(r => r.Id == Guid.Parse(id));

		if (role == null)
		{
			return null;
		}

		// Get the user's current role using your GetRoleAsync method
		string? currentRoleName = await user.GetRoleAsync(this._userManager);

		if (currentRoleName == role.Name)
		{
			return role.Name; // User already has the selected role, no need to update
		}

		// Remove the user from all current roles
		var removeResult = await this._userManager.RemoveFromRolesAsync(user, await this._userManager.GetRolesAsync(user));

		if (!removeResult.Succeeded)
		{
			return null; // Return null if unable to remove user from current roles
		}

		// Add the user to the new role
		var addResult = await this._userManager.AddToRoleAsync(user, role.Name);

		if (addResult.Succeeded)
		{
			// Update the Role property of ApplicationUser (if necessary)
			user.Role = role;

			// Save changes to update the Role property in the database
			await this._dbContext.SaveChangesAsync();

			// Update the corresponding table based on the new role
			await this.UpdateUserRoleTableAsync(user, role.Name);

			// Remove the user from the corresponding table based on the current role
			await this.RemoveUserFromCurrentRoleTableAsync(user, currentRoleName);

			return role.Name;
		}

		return null; // Return null if unable to add role
	}

	/// <summary>
	///     Retrieves all roles from the database and returns them as an enumerable collection of UserRolesViewModel objects.
	/// </summary>
	/// <returns>An enumerable collection of UserRolesViewModel objects representing all roles.</returns>
	public async Task<IEnumerable<UserRolesViewModel>> GetAllRolesAsync()
	{
		IEnumerable<UserRolesViewModel> allRoles = await this._roleManager
			.Roles
			.Select(r => new UserRolesViewModel
			{
				Id = r.Id.ToString(),
				Name = r.Name
			})
			.AsNoTracking()
			.ToArrayAsync(); // Retrieve all roles from the RoleManager and map them to UserRolesViewModel objects

		return allRoles;
	}
    public async Task<UserEditFormModel?> EditAsync(string id, UserEditFormModel model)
    {
        var user = await this.GetUserById(id);

        if (user == null)
        {
            return null;
        }

        user.FirstName = model.FirstName;
		user.LastName = model.LastName;
        user.BirthDate = DateTime.ParseExact(model.BirthDate, BIRTH_DATE_FORMAT, CultureInfo.InvariantCulture);
		user.Address = model.Address;
        user.PhoneNumber = model.PhoneNumber;
		user.Email = model.Email.ToLower();
        user.NormalizedEmail = model.Email.ToUpper();
        user.IsActive = model.IsActive;

        await this._dbContext.SaveChangesAsync();

        return model;
    }

    public async Task<UserEditFormModel?> GetForEditAsync(string id)
	{
		var user = await this.GetUserById(id);

		if (user == null)
		{
			return null;
		}

		var viewModel = new UserEditFormModel
		{
			Id = user.Id.ToString(),
			FirstName = user.FirstName,
			LastName = user.LastName,
			BirthDate = user.GetBirthDate(),
			Address = user.Address,
			PhoneNumber = user.PhoneNumber,
			Email = user.Email,
			IsActive = user.IsActive
		};

		return viewModel;
	}

	public async Task<ApplicationUser?> GetUserById(string id)
	{
		return await this._dbContext
			.Users
			.FindAsync(Guid.Parse(id));
	}
}