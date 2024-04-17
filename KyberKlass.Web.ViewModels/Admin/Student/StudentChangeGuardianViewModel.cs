namespace KyberKlass.Web.ViewModels.Admin.Student;

using User;

public class StudentChangeGuardianViewModel
{
    public StudentChangeGuardianViewModel()
    {
        this.AvailableGuardians = new HashSet<BasicViewModel>();
    }

    public UserDetailsViewModel? UserDetails { get; set; }

    public IEnumerable<BasicViewModel> AvailableGuardians { get; set; }
}