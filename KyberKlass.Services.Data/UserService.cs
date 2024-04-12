namespace KyberKlass.Services.Data;

using System.Globalization;
using Interfaces;
using KyberKlass.Data;
using KyberKlass.Data.Models;
using Web.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.Admin.Guardian;
using Web.ViewModels.Admin.User;
using static Common.FormattingConstants;
using static KyberKlass.Common.CustomMessageConstants;
using Student = KyberKlass.Data.Models.Student;

/// <summary>
/// Service for managing user-related operations.
/// </summary>
public class UserService : IUserService
{
	private readonly KyberKlassDbContext _dbContext;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly RoleManager<IdentityRole<Guid>> _roleManager;
	private readonly IGuardianService _guardianService;
	private readonly ISchoolService _schoolService;

	public UserService(KyberKlassDbContext dbContext,
		UserManager<ApplicationUser> userManager,
		RoleManager<IdentityRole<Guid>> roleManager,
		IGuardianService guardianService,
		ISchoolService schoolService)
	{
		this._dbContext = dbContext;
		this._userManager = userManager;
		this._roleManager = roleManager;
		this._guardianService = guardianService;
		this._schoolService = schoolService;
	}

	// Helper method to retrieve all students assigned to a guardian
	private async Task<IEnumerable<BasicViewModel>> GetAllStudentsAssignedByGuardianIdAsync(Guardian guardian)
	{
		IEnumerable<BasicViewModel> guardianStudents = await this._dbContext
			.Students
			.Where(s => s.Guardian.Id == guardian.Id)
			.Select(s => new BasicViewModel
			{
				Id = s.Id.ToString(),
				Name = s.ApplicationUser.GetFullName()
			})
			.AsNoTracking()
			.ToArrayAsync();

		return guardianStudents;
	}

	/// <inheritdoc />
	public async Task<IEnumerable<UserViewModel>> AllAsync()
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

		IEnumerable<UserViewModel> userViewModels = usersWithRoles
			.Select(u => new UserViewModel
			{
				Id = u.User.Id.ToString(),
				FullName = u.User.GetFullName(),
				Email = u.User.Email,
				Role = u.Roles.FirstOrDefault() ?? "No Role Assigned",
				IsActive = u.User.IsActive
			})
			.ToArray();

		return userViewModels; // TODO Make this method better
	}

	/// <inheritdoc />
	public async Task<UserDetailsViewModel?> GetDetailsAsync(string id)
	{
		var user = await this.GetUserById(id); 

		if (user == null)
		{
			return null;
		}

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

		if (viewModel.Role == "Student")
		{
			var guardian = await this._dbContext
				.Guardians
				.Include(g => g.ApplicationUser)
				.AsNoTracking()
				.FirstOrDefaultAsync(g => g.Students.Any(s => s.Id == user.Id));

			if (guardian != null)
			{
				IEnumerable<BasicViewModel> guardianStudents = await this.GetAllStudentsAssignedByGuardianIdAsync(guardian);

				viewModel.Guardian = new GuardianViewModel
				{
					Id = guardian.Id.ToString(),
					FullName = guardian.ApplicationUser.GetFullName(),
					Address = guardian.ApplicationUser.Address,
					Email = guardian.ApplicationUser.Email,
					PhoneNumber = guardian.ApplicationUser.PhoneNumber,
					Students = guardianStudents
				};
			}

		}
		else if (viewModel.Role == "Guardian")
		{
			IEnumerable<BasicViewModel> students = await this._dbContext
				.Students
				.Where(s => s.Guardian.Id == user.Id)
				.Select(s => new BasicViewModel
				{
					Id = s.Id.ToString().ToLower(),
					Name = s.ApplicationUser.GetFullName()
				})
				.AsNoTracking()
				.ToArrayAsync();

			viewModel.Students = students;
		}

		return viewModel;
	}

	/// <inheritdoc />
	public async Task<UserUpdateRoleViewModel?> GetForUpdateRoleAsync(string id)
	{
		var user = await this.GetUserById(id); 

		if (user == null)
		{
			return null; 
		}

		string currentRoleName = await user.GetRoleAsync(this._userManager);

		IEnumerable<UserRolesViewModel> availableRoles = await this.GetAllRolesAsync();

		var viewModel = new UserUpdateRoleViewModel
		{
			Id = user.Id.ToString(),
			FullName = user.GetFullName(),
			Email = user.Email,
			IsActive = user.IsActive,
			PreviousRoleName = currentRoleName,
			CurrentRoleName = currentRoleName,
			AvailableRoles = availableRoles
		};

		if (currentRoleName == "Guardian")
		{
			IEnumerable<BasicViewModel> students = await this._dbContext.Students
				.Where(s => s.Guardian.Id == user.Id)
				.Select(s => new BasicViewModel
				{
					Id = s.Id.ToString(),
					Name = s.ApplicationUser.GetFullName()
				})
				.AsNoTracking()
				.ToArrayAsync();

			viewModel.Students = students;
		}

		return viewModel;
	}

	/// <inheritdoc />
	public async Task<IEnumerable<BasicViewModel>> GetAllGuardiansAsync()
	{
		IList<ApplicationUser> guardians = await this._userManager.GetUsersInRoleAsync("Guardian");

		IEnumerable<BasicViewModel> guardianViewModels = guardians
			.Select(g => new BasicViewModel
			{
				Id = g.Id.ToString(),
				Name = g.GetFullName()
			})
			.ToArray();

		return guardianViewModels;
	}

	/// <inheritdoc />
	public async Task<bool> IsTeacherAssignedToClassroomAsync(string userId)
	{
		bool isAssigned = await this._dbContext.Classrooms
			.AsNoTracking()
			.AnyAsync(c => c.TeacherId == Guid.Parse(userId));

		return isAssigned;
	}

	/// <inheritdoc />
	public async Task<bool> IsGuardianAssignedToStudentAsync(string userId)
	{
		bool isAssigned = await this._dbContext.Students
			.AsNoTracking()
			.AnyAsync(s => s.Guardian.Id == Guid.Parse(userId));

		return isAssigned;
	}

	/// <inheritdoc />
	public async Task<bool> UpdateRoleAsync(string userId, string roleId, string? guardianId,  string? schoolId, string? classroomId)
	{
		var user = await this.GetUserById(userId);
		if (user == null)
			return false;

		IdentityRole<Guid>? roleToUpdate = await this._roleManager.FindByIdAsync(roleId); // Role to update to
		if (roleToUpdate == null)
			return false;

		string currentRoleName = await user.GetRoleAsync(this._userManager); // Current user role

		if (currentRoleName == "Guardian") // If current user role is "Guardian"
		{
			var guardian = await this._dbContext.Guardians.FindAsync(user.Id); // Get the guardian entity

			if (guardian != null && guardian.Students.Any()) // If the guardian exists and has students assigned
			{
				return false; // Return false
			}
		}

		// If roleToUpdate is "Student" and ID'S required are not null
		if (roleToUpdate.Name == "Student" && guardianId != null && schoolId != null && classroomId != null)
		{
			// Validations for guardian school and classroom to assign to "Student"
			var guardianToAssign = await this._guardianService.GetByIdAsync(guardianId);
			var schoolToAssign = await this._schoolService.GetByIdAsync(schoolId);
			bool classroomExistsInSchool = await this._schoolService.ClassroomExistsInSchoolAsync(schoolId, classroomId);

			// If any of the validations fail return false
			if (guardianToAssign == null || schoolToAssign == null || classroomExistsInSchool == false)
			{
				return false;
			}
		}

		// Start a transaction
		await using var transaction = await this._dbContext.Database.BeginTransactionAsync();

		try
		{
            if (currentRoleName != "No Role Assigned")
            {
                // Update user's role in the identity system
                var removeResult = await this._userManager.RemoveFromRolesAsync(user, await this._userManager.GetRolesAsync(user));

                if (removeResult.Succeeded == false)
                {
                    // Roll back transaction if removing roles fails
                    await transaction.RollbackAsync();
                    return false;
                }
            }

			var addResult = await this._userManager.AddToRoleAsync(user, roleToUpdate.Name);

			if (addResult.Succeeded == false)
			{
				// Roll back transaction if adding roles fails
				await transaction.RollbackAsync();
				return false;
			}

			user.Role = roleToUpdate;

			// Update user role-related tables in the database
			// Remove user from the current role table based on the previous role
			switch (currentRoleName)
			{
				case "Teacher":
					var teacherToRemove = await this._dbContext.Teachers.FindAsync(user.Id);
					if (teacherToRemove != null)
						this._dbContext.Teachers.Remove(teacherToRemove);

					break;
				case "Guardian":
					var guardianToRemove = await this._dbContext.Guardians.FindAsync(user.Id);
					if (guardianToRemove != null)
						this._dbContext.Guardians.Remove(guardianToRemove);

					break;
				case "Student":
					var studentToRemove = await this._dbContext.Students.FindAsync(user.Id);
					if (studentToRemove != null)
						this._dbContext.Students.Remove(studentToRemove);

					break;
			}

			// Update user role table based on the new role
			switch (roleToUpdate.Name)
			{
				case "Teacher":
					this._dbContext.Teachers.Add(new Teacher { Id = user.Id });
					break;
				case "Guardian":
					this._dbContext.Guardians.Add(new Guardian { Id = user.Id });
					break;
				case "Student":
					this._dbContext.Students.Add(new Student
					{
						Id = user.Id,
						GuardianId = Guid.Parse(guardianId!),
						SchoolId = Guid.Parse(schoolId!),
						ClassroomId = Guid.Parse(classroomId!)
					});

					break;
			}

			// Save changes to commit removal and addition of user from/to role tables
			await this._dbContext.SaveChangesAsync();

			// Commit the transaction
			await transaction.CommitAsync();

			return true;
		}
		catch (Exception)
		{
			// Roll back transaction in case of any exception
			await transaction.RollbackAsync();
			return false;
		}
	}

	/// <inheritdoc />
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

	/// <inheritdoc />
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

	/// <inheritdoc />
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

	/// <inheritdoc />
	public async Task<ApplicationUser?> GetUserById(string id)
	{
		return await this._dbContext
			.Users
			.FindAsync(Guid.Parse(id));
	}

	/// <inheritdoc />
	public async Task<string?> GetRoleNameByIdAsync(string id)
	{
		IdentityRole<Guid>? role = await this._roleManager.FindByIdAsync(id);
		string? roleName = role.Name;

		return roleName;
	}
}