namespace KyberKlass.Web.ViewModels.Admin.Classroom;

public class ManageClassroomsViewModel
{
	public string SchoolId { get; set; } = null!;

	public string SchoolName { get; set; } = null!;

	public IEnumerable<ClassroomViewModel> Classrooms { get; set; } = new List<ClassroomViewModel>();
}