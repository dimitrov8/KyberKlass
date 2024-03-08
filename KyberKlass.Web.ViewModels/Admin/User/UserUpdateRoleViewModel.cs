namespace KyberKlass.Web.ViewModels.Admin.User;

using System.ComponentModel.DataAnnotations;

public class UserUpdateRoleViewModel
{
	public string Id { get; set; } = null!;

	public string FullName { get; set; } = null!;

	public string Email { get; set; } = null!;

	public bool IsActive { get; set; }

	public string? CurrentRoleName { get; set; }

	[Required(ErrorMessage = "Please select a role.")]
	public string? RoleId { get; set; }

	public IEnumerable<UserRolesViewModel> AvailableRoles { get; set; } = new HashSet<UserRolesViewModel>();
}