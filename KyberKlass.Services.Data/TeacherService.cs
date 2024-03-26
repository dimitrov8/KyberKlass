namespace KyberKlass.Services.Data;

using Interfaces;
using KyberKlass.Data;
using KyberKlass.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.Admin.User;

public class TeacherService : ITeacherService
{
	private readonly KyberKlassDbContext _dbContext;
	private readonly UserManager<ApplicationUser> _userManager;

	public TeacherService(KyberKlassDbContext dbContext, UserManager<ApplicationUser> userManager)
	{
		this._dbContext = dbContext;
		this._userManager = userManager;
	}

	/// <summary>
	///     Retrieves all users assumed to be teachers from the database.
	///     The role of "Teacher" is hardcoded in the view model, as we are certain that
	///     the fetched users have this role. This approach avoids unnecessary queries
	///     to fetch the role for each user, leading to improved performance and efficiency.
	/// </summary>
	/// <returns>
	///     A collection of user view models representing teachers, or null if no teachers are found.
	/// </returns>
	public async Task<List<UserViewModel>?> AllAsync()
	{
		string teacherRoleName = "Teacher";

		var teacherRoleId = await this._dbContext
			.Roles
			.AsNoTracking()
			.Where(r => r.Name == teacherRoleName)
			.Select(r => r.Id)
			.FirstOrDefaultAsync();

		if (teacherRoleId != Guid.Empty)
		{
			// Retrieve all users who have the "Teacher" role
			List<ApplicationUser> teachers = await this._dbContext
				.Users
				.Where(user => this._dbContext
					.UserRoles
					.Any(userRole => userRole.UserId == user.Id && userRole.RoleId == teacherRoleId))
				//.Include(user => user.Role)
				.AsNoTracking()
				.ToListAsync();

			List<UserViewModel> teacherViewModels = teachers
				.Select(t => new UserViewModel
				{
					Id = t.Id.ToString(),
					Email = t.Email,
					FullName = t.GetFullName(),
					Role = teacherRoleName, // Hardcode the role as "Teacher" to avoid unnecessary role queries
					IsActive = t.IsActive
				})
				.ToList();

			return teacherViewModels;
		}

		return null;
	}

	/// <summary>
	///     Asynchronously retrieves a collection of basic user view models representing unassigned teachers.
	/// </summary>
	/// <returns>
	///     A collection of user basic view models representing unassigned teachers.
	///     If no unassigned teachers are found, an empty collection is returned.
	/// </returns>
	public async Task<IEnumerable<UserBasicViewModel>> GetUnassignedTeachersAsync()
	{
		IList<ApplicationUser> allTeachers = await this._userManager.GetUsersInRoleAsync("Teacher");

		if (allTeachers.Any() == false)
		{
			return Enumerable.Empty<UserBasicViewModel>();
		}

		List<Guid> assignedTeacherIds = await this._dbContext.Classrooms
			.Select(c => c.TeacherId)
			.ToListAsync();

		List<UserBasicViewModel> unassignedTeachers = allTeachers
			.Where(t => assignedTeacherIds.Contains(t.Id) == false)
			.Select(t => new UserBasicViewModel
			{
				Id = t.Id.ToString(),
				Name = t.GetFullName()
			})
			.ToList();

		return unassignedTeachers;
	}
}