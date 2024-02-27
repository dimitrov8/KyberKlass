namespace KyberKlass.Web.ViewModels.Admin.School;

using System.ComponentModel.DataAnnotations;

public class SchoolViewModel
{
	public string Id { get; set; } = null!;

	public string Name { get; set; } = null!;

	public string Address { get; set; } = null!;

	public string Email { get; set; } = null!;

	[Display(Name = "Phone Number")]
	public string PhoneNumber { get; set; } = null!;

	public bool IsDeleted { get; set; }
}