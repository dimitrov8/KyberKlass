namespace KyberKlass.Services.Data;

using Interfaces;
using KyberKlass.Data;
using KyberKlass.Data.Models;
using KyberKlass.Web.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.Admin.Student;
using Web.ViewModels.Admin.User;

public class StudentService : IStudentService
{
	private readonly KyberKlassDbContext _dbContext;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IUserService _userService;
	private readonly IGuardianService _guardianService;

	public StudentService(KyberKlassDbContext dbContext,
		UserManager<ApplicationUser> userManager,
		IUserService userService,
		IGuardianService guardianService)
	{
		this._dbContext = dbContext;
		this._userManager = userManager;
		this._userService = userService;
		this._guardianService = guardianService;
	}

	public async Task<IEnumerable<UserViewModel>?> AllAsync()
	{
		string studentRoleName = "Student";

		var studentRoleId = await this._dbContext
			.Roles
			.AsNoTracking()
			.Where(r => r.Name == studentRoleName)
			.Select(r => r.Id)
			.FirstOrDefaultAsync();

		if (studentRoleId != Guid.Empty)
		{
			// Retrieve all users who have the "Teacher" role
			List<ApplicationUser> students = await this._dbContext
				.Users
				.Where(user => this._dbContext
					.UserRoles
					.Any(userRole => userRole.UserId == user.Id && userRole.RoleId == studentRoleId))
				//.Include(user => user.Role)
				.AsNoTracking()
				.ToListAsync();

			List<UserViewModel> studentViewModels = students
				.Select(t => new UserViewModel
				{
					Id = t.Id.ToString(),
					Email = t.Email,
					FullName = t.GetFullName(),
					Role = studentRoleName, // Hardcode the role as "Student" to avoid unnecessary role queries
					IsActive = t.IsActive
				})
				.ToList();

			return studentViewModels;
		}

		return null;
	}

	public Task<Student?> GetByIdASync(string id)
	{
		return this._dbContext
			.Students
			.Include(s => s.Guardian)
			.FirstOrDefaultAsync(s => s.Id == Guid.Parse(id));
	}

	/// <summary>
	///     Retrieves a collection of basic user view models representing unassigned students asynchronously.
	/// </summary>
	/// <returns>
	///     A collection of user basic view models representing unassigned students.
	///     If no unassigned students are found, an empty collection is returned.
	/// </returns>
	public async Task<IEnumerable<UserBasicViewModel>> GetUnassignedStudentsAsync()
	{
		IList<ApplicationUser> allStudents = await this._userManager.GetUsersInRoleAsync("Student"); // Retrieve all users assigned the role of "Student"

		if (allStudents.Any() == false) // Check if there are any students found
		{
			return Enumerable.Empty<UserBasicViewModel>();
		}

		List<Guid> assignedStudentIds = await this._dbContext.Classrooms
			.SelectMany(c => c.Students)
			.Select(s => s.Id)
			.ToListAsync(); // Retrieve the IDs of students who are assigned to classrooms

		List<UserBasicViewModel> unassignedStudents = allStudents
			.Where(s => assignedStudentIds.Contains(s.Id) == false)
			.Select(s => new UserBasicViewModel
			{
				Id = s.Id.ToString(),
				Name = s.GetFullName()
			})
			.ToList(); // Filter out the unassigned students

		return unassignedStudents; // Return the collection of unassigned students
	}

	public async Task<StudentChangeGuardianViewModel> GetStudentChangeGuardianAsync(string userId)
	{
		var userDetails = await this._userService.GetDetailsAsync(userId);
		IEnumerable<UserBasicViewModel> availableGuardians = await this._userService.GetAllGuardiansAsync();

		var viewModel = new StudentChangeGuardianViewModel
		{
			UserDetails = userDetails,
			AvailableGuardians = availableGuardians
		};

		return viewModel;
	}

	public async Task<bool> StudentChangeGuardianAsync(string userId, string guardianId)
	{
		Student? student = await this.GetByIdASync(userId);
		Guardian? newGuardian = await this._guardianService.GetByIdAsync(guardianId);

		if (student == null || newGuardian == null)
		{
			return false;
		}

		var previousGuardianId = student.Guardian.Id;

		if (previousGuardianId == newGuardian.Id)
		{
			return false;
		}

		student.GuardianId = Guid.Empty;
		student.GuardianId = newGuardian.Id;

		await this._dbContext.SaveChangesAsync();

		return true;
	}
}