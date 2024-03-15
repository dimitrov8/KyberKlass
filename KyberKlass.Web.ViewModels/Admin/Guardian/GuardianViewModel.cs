namespace KyberKlass.Web.ViewModels.Admin.Guardian;

public class GuardianViewModel
{
	public GuardianViewModel()
	{
		this.StudentNames = new HashSet<string>();
	}

	public string FullName { get; set; } = null!;

	public string Email { get; set; } = null!;

	public string PhoneNumber { get; set; } = null!;

	public IEnumerable<string> StudentNames { get; set; }
}