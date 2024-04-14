namespace KyberKlass.Web.ViewModels.Admin.Classroom;

public class ManageClassroomsViewModel
{
	public ManageClassroomsViewModel()
	{
		this.Classrooms = new HashSet<ClassroomDetailsViewModel>();
	}

	public string SchoolId { get; set; } = null!;

	public string SchoolName { get; set; } = null!;

	public IEnumerable<ClassroomDetailsViewModel> Classrooms { get; set; }
}