namespace KyberKlass.Web.ViewModels.Admin.School;

using System.ComponentModel.DataAnnotations;
using Classroom;

public class SchoolViewModel
{
	public SchoolViewModel()
	{
		this.Classrooms = new HashSet<ClassroomViewModel>();
	}

	public string Id { get; set; } = null!;

	public string Name { get; set; } = null!;

	public string Address { get; set; } = null!;

	public string Email { get; set; } = null!;

	[Display(Name = "Phone Number")]
	public string PhoneNumber { get; set; } = null!;

	public bool IsActive { get; set; }

	public ICollection<ClassroomViewModel> Classrooms { get; set; } 
}