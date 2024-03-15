namespace KyberKlass.Services.Data;

using Interfaces;
using KyberKlass.Data;
using KyberKlass.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.Admin.User;

public class StudentService : IStudentService
{
	private readonly KyberKlassDbContext _dbContext;
	private readonly UserManager<ApplicationUser> _userManager;

	public StudentService(KyberKlassDbContext dbContext, UserManager<ApplicationUser> userManager)
	{
		this._dbContext = dbContext;
		this._userManager = userManager;
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
}