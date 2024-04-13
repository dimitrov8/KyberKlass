namespace KyberKlass.Web.ViewModels.Admin.Classroom;

public class ClassroomDetailsViewModel
{
    public ClassroomDetailsViewModel()
    {
        this.Students = new HashSet<BasicViewModel>();
    }

    public string Id { get; set; } = null!;

    public string? SchoolId { get; set; } // Optional only used in the controller "Classroom" "Details" action.

    public string Name { get; set; } = null!;

    public string TeacherName { get; set; } = null!;

    public bool IsActive { get; set; }

	public ICollection<BasicViewModel> Students { get; set; }

    public int StudentsCount => this.Students.Count;
}