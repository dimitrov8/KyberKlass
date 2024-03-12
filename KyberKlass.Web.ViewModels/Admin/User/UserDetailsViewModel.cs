namespace KyberKlass.Web.ViewModels.Admin.User;

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
}