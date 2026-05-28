namespace KyberKlass.Web.ViewModels.Admin.User;

public class UserUpdateRoleViewModel
{
    public string Id { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool IsActive { get; set; }

    public string? PreviousRoleName { get; set; }

    public string? CurrentRoleName { get; set; }

    public IEnumerable<BasicViewModel>? Students { get; set; }

    public string RoleId { get; set; } = null!;

    public string? GuardianId { get; set; }

    public string? SchoolId { get; set; }

    public string? ClassroomId { get; set; }

    // Additional property to hold available roles
    public IEnumerable<UserRolesViewModel> AvailableRoles { get; set; } = new HashSet<UserRolesViewModel>();

    // Additional property to hold available guardians (only for students)
    public IEnumerable<BasicViewModel> AvailableGuardians { get; set; } = new HashSet<BasicViewModel>();

    // Additional property to hold available classrooms (only for students)
    public IEnumerable<BasicViewModel> AvailableSchools { get; set; } = new HashSet<BasicViewModel>();

    // Additional property to hold available classrooms (only for students)
    public IEnumerable<BasicViewModel> AvailableClassrooms { get; set; } = new HashSet<BasicViewModel>();
}