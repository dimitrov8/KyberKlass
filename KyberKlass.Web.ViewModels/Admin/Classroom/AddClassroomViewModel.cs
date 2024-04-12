namespace KyberKlass.Web.ViewModels.Admin.Classroom;

using System.ComponentModel.DataAnnotations;
using static Common.EntityValidations.Classroom;

public class AddClassroomViewModel
{
	public AddClassroomViewModel()
	{
		this.UnassignedTeachers = new HashSet<UserBasicViewModel>();
	}

	[Required(ErrorMessage = "Classroom name is required.")]
	[StringLength(MAX_NAME_LENGTH, MinimumLength = MIN_NAME_LENGTH)]
	public string Name { get; set; } = null!;

	[Required(ErrorMessage = "Please select a teacher")]
	public string TeacherId { get; set; } = null!;

	[Required]
	public bool IsActive { get; set; } = true;

	[Required]
	public string SchoolId { get; set; } = null!;

	public IEnumerable<UserBasicViewModel> UnassignedTeachers { get; set; }
}