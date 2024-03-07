namespace KyberKlass.Web.ViewModels.Admin.User;

public class UserUpdateRoleViewModel
{
    public string Id { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool IsActive { get; set; }

    public string? CurrentRoleName { get; set; }

    public string? RoleId { get; set; }

    public IEnumerable<UserRolesViewModel> AvailableRoles { get; set; } = new HashSet<UserRolesViewModel>();
}