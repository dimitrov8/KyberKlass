namespace KyberKlass.Web.ViewModels.Admin.User;

using System.Diagnostics;
using Guardian;
using School;

public class UserDetailsViewModel
{
    public UserDetailsViewModel()
    {
        this.Students = new HashSet<BasicViewModel>();
    }

    public string Id { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string BirthDate { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string IsActive { get; set; } = null!;

    // Additional properties if user has student role
    // Properties for student's guardian information
    public GuardianViewModel? Guardian { get; set; }
    public IEnumerable<BasicViewModel>? Students { get; set; }

    // Property for student's school information
    public IEnumerable<SchoolViewModel>? School { get; set; }
}