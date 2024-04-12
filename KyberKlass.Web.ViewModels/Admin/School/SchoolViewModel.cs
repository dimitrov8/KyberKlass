namespace KyberKlass.Web.ViewModels.Admin.School;

using System.ComponentModel.DataAnnotations;
using Classroom;

public class SchoolViewModel
{
	public SchoolViewModel()
	{
		this.Classrooms = new HashSet<ClassroomDetailsViewModel>();
	}

	public string Id { get; set; } = null!;

	public string Name { get; set; } = null!;

	public string Address { get; set; } = null!;

	public string Email { get; set; } = null!;

	[Display(Name = "Phone Number")]
	public string PhoneNumber { get; set; } = null!;

	public bool IsActive { get; set; }

	[Display(Name = "Total Classrooms")]
	public int TotalClassrooms => this.Classrooms.Count();

	[Display(Name = "Total Students")]
	public int TotalStudents => this.Classrooms.Sum(s => s.StudentsCount);

	public IEnumerable<ClassroomDetailsViewModel> Classrooms { get; set; }
}