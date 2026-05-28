#region

using System.ComponentModel.DataAnnotations;

using KyberKlass.Web.ViewModels.Admin.Classroom;

#endregion

namespace KyberKlass.Web.ViewModels.Admin.School;

public class SchoolDetailsViewModel
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Email { get; set; } = null!;

    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; } = null!;

    public bool IsActive { get; set; }

    [Display(Name = "Total Classrooms")]
    public int TotalClassrooms => Classrooms.Count();

    [Display(Name = "Total Students")]
    public int TotalStudents => Classrooms.Sum(selector: s => s.StudentsCount);

    public IEnumerable<ClassroomDetailsViewModel> Classrooms { get; set; } = new HashSet<ClassroomDetailsViewModel>();

    public string? SearchTerm { get; set; }
}