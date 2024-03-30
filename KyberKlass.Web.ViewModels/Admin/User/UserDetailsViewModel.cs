namespace KyberKlass.Web.ViewModels.Admin.User;

using Guardian;

public class UserDetailsViewModel
{
	public string Id { get; set; } = null!;

	public string FullName { get; set; } = null!;

	public string BirthDate { get; set; } = null!;

	public string Address { get; set; } = null!;

	public string PhoneNumber { get; set; } = null!;

	public string Email { get; set; } = null!;

	public string Role { get; set; } = null!;

	public string IsActive { get; set; } = null!;

	// Additional property for guardian information
	public GuardianViewModel? Guardian { get; set; }

	public IEnumerable<UserBasicViewModel>? Students { get; set; }
}