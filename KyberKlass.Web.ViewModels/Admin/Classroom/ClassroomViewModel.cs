namespace KyberKlass.Web.ViewModels.Admin.Classroom;

public class ClassroomViewModel
{
	public string Id { get; set; } = null!;

	public string Name { get; set; } = null!;

	public string TeacherName { get; set; } = null!;

	public int StudentsCount { get; set; }
}