namespace KyberKlass.Web.ViewModels.Admin.Classroom;

using System.Runtime.CompilerServices;
using User;

public class ClassroomViewModel
{
	public ClassroomViewModel()
	{
		this.Students = new HashSet<UserBasicViewModel>();
	}

	public string Id { get; set; } = null!;

	public string Name { get; set; } = null!;

	public string TeacherName { get; set; } = null!;

	public ICollection<UserBasicViewModel> Students { get; set; }

	public int StudentsCount => this.Students.Count;
}