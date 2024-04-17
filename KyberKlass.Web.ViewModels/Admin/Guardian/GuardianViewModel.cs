namespace KyberKlass.Web.ViewModels.Admin.Guardian;

public class GuardianViewModel
{
    public GuardianViewModel()
    {
        this.Students = new HashSet<BasicViewModel>();
    }

    public string Id { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public IEnumerable<BasicViewModel> Students { get; set; }
}