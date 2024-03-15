namespace KyberKlass.Web.ViewModels.Admin.User;

using System.ComponentModel.DataAnnotations;
using Classroom;

public class UserUpdateRoleViewModel
{
	public UserUpdateRoleViewModel()
	{
		this.AvailableRoles = new HashSet<UserRolesViewModel>();
		this.AvailableGuardians = new HashSet<UserBasicViewModel>();
		this.AvailableClassrooms = new HashSet<ClassroomViewModel>();
	}

	public string Id { get; set; } = null!;

	public string FullName { get; set; } = null!;

	public string Email { get; set; } = null!;

	public bool IsActive { get; set; }

	public string? CurrentRoleName { get; set; }

	[Required(ErrorMessage = "Please select a role.")]
	public string? RoleId { get; set; }

	public string? GuardianId { get; set; }

	public string? ClassroomId { get; set; }

	public IEnumerable<UserRolesViewModel> AvailableRoles { get; set; }

	// Additional property to hold available guardians (only for students)
	public IEnumerable<UserBasicViewModel> AvailableGuardians { get; set; }

	// Additional property to hold available classrooms (only for students)
	public IEnumerable<ClassroomViewModel> AvailableClassrooms { get; set; }
}