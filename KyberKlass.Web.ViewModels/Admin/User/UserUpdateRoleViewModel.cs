namespace KyberKlass.Web.ViewModels.Admin.User;

public class UserUpdateRoleViewModel
{
    public UserUpdateRoleViewModel()
    {
        this.AvailableRoles = new HashSet<UserRolesViewModel>();
        this.AvailableGuardians = new HashSet<UserBasicViewModel>();
        this.AvailableSchools = new HashSet<UserBasicViewModel>();
        this.AvailableClassrooms = new HashSet<UserBasicViewModel>();
    }

    public string Id { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool IsActive { get; set; }

    public string? PreviousRoleName { get; set; }

    public string? CurrentRoleName { get; set; }

    public IEnumerable<UserBasicViewModel>? Students { get; set; }

    public string RoleId { get; set; } = null!;

    public string? GuardianId { get; set; }

    public string? SchoolId { get; set; }

    public string? ClassroomId { get; set; }

    // Additional property to hold available roles
    public IEnumerable<UserRolesViewModel> AvailableRoles { get; set; }

    // Additional property to hold available guardians (only for students)
    public IEnumerable<UserBasicViewModel> AvailableGuardians { get; set; }

    // Additional property to hold available classrooms (only for students)
    public IEnumerable<UserBasicViewModel> AvailableSchools { get; set; }

    // Additional property to hold available classrooms (only for students)
    public IEnumerable<UserBasicViewModel> AvailableClassrooms { get; set; }
}