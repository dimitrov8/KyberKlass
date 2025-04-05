using KyberKlass.Web.ViewModels.Admin.User;

namespace KyberKlass.Web.ViewModels.Admin.Student;
public class StudentChangeGuardianViewModel
{
    public StudentChangeGuardianViewModel()
    {
        AvailableGuardians = new HashSet<BasicViewModel>();
    }

    public UserDetailsViewModel? UserDetails { get; set; }

    public IEnumerable<BasicViewModel> AvailableGuardians { get; set; }
}