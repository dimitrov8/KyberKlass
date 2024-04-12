namespace KyberKlass.Web.ViewModels.Admin.Classroom;

public class ClassroomDetailsViewModel
{
    public ClassroomDetailsViewModel()
    {
        this.Students = new HashSet<UserBasicViewModel>();
    }

    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string TeacherName { get; set; } = null!;

    public ICollection<UserBasicViewModel> Students { get; set; }

    public int StudentsCount => this.Students.Count;
}