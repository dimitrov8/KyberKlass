namespace KyberKlass.Web.ViewModels.Admin.Student;

using User;

public class StudentChangeGuardianViewModel
{
	public StudentChangeGuardianViewModel()
	{
		this.AvailableGuardians = new HashSet<UserBasicViewModel>();
	}

	public UserDetailsViewModel? UserDetails { get; set; }

	public IEnumerable<UserBasicViewModel> AvailableGuardians { get; set; }
}