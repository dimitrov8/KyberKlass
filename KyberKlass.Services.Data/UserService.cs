namespace KyberKlass.Services.Data;

using System.Globalization;
using Interfaces;
using KyberKlass.Data;
using KyberKlass.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.Admin.Classroom;
using Web.ViewModels.Admin.User;
using static Common.FormattingConstants;

public class UserService : IUserService
{
	private readonly KyberKlassDbContext _dbContext;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly RoleManager<IdentityRole<Guid>> _roleManager;

	public UserService(KyberKlassDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
	{
		this._dbContext = dbContext;
		this._userManager = userManager;
		this._roleManager = roleManager;
    }

	private async Task UpdateStudentAsync(ApplicationUser user, string? guardianId, string? classroomId)
	{
		this._dbContext.Students.Add(new Student
		{
			Id = user.Id,
			GuardianId = guardianId != null ? Guid.Parse(guardianId) : default,
			ClassroomId = classroomId != null ? Guid.Parse(classroomId) : default
		});

		await this._dbContext.SaveChangesAsync();
	}

	private async Task UpdateTeacherAsync(ApplicationUser user)
	{
		this._dbContext.Teachers.Add(new Teacher { Id = user.Id });
		await this._dbContext.SaveChangesAsync();
	}

	private async Task UpdateGuardianAsync(ApplicationUser user)
	{
		this._dbContext.Guardians.Add(new Guardian { Id = user.Id });
		await this._dbContext.SaveChangesAsync();
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

		// Create a view model for updating user role
		var viewModel = new UserUpdateRoleViewModel
		{
			Id = user.Id.ToString(),
			FullName = user.GetFullName(),
			Email = user.Email,
			IsActive = user.IsActive,
			CurrentRoleName = await user.GetRoleAsync(this._userManager), // Retrieve the user's current role asynchronously
			AvailableRoles = availableRoles, // Assign available roles to the view model
        };

		return viewModel;
	}

	public async Task<IEnumerable<UserBasicViewModel>> GetAllGuardiansAsync()
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

	private async Task UpdateUserRoleTableAsync(ApplicationUser user, string roleName, string? guardianId, string? classroomId)
	{
		switch (roleName)
		{
			case "Teacher":
				await this.UpdateTeacherAsync(user);
				break;
			case "Guardian":
				await this.UpdateGuardianAsync(user);
				break;
			case "Student":
				await this.UpdateStudentAsync(user, guardianId, classroomId);
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

	public async Task<string?> UpdateRoleAsync(string userId, string roleId, string? guardianId, string? classroomId)
	{
		var user = await this.GetUserById(userId);
		if (user == null)
			return null;

		IdentityRole<Guid>? role = await this._roleManager.Roles.FirstOrDefaultAsync(r => r.Id == Guid.Parse(roleId));
		if (role == null)
			return null;

		string currentRoleName = await user.GetRoleAsync(this._userManager);
		if (currentRoleName == role.Name)
			return role.Name;

		var removeResult = await this._userManager.RemoveFromRolesAsync(user, await this._userManager.GetRolesAsync(user));
        if (removeResult.Succeeded == false)
			return null;

		var addResult = await this._userManager.AddToRoleAsync(user, role.Name);
		if (addResult.Succeeded == false)
			return null;

		user.Role = role;
		await this._dbContext.SaveChangesAsync();

		await this.UpdateUserRoleTableAsync(user, role.Name, guardianId, classroomId);
		await this.RemoveUserFromCurrentRoleTableAsync(user, currentRoleName);

		return role.Name;
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