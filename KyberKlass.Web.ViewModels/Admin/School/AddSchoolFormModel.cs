namespace KyberKlass.Web.ViewModels.Admin.School;

using System.ComponentModel.DataAnnotations;
using static Common.EntityValidations.School;

public class AddSchoolFormModel
{
	[Required]
	public Guid Id { get; set; }

	[Required]
	[StringLength(MAX_NAME_LENGTH, MinimumLength = MIN_NAME_LENGTH)]
	public string Name { get; set; } = null!;

	[Required]
	[StringLength(MAX_ADDRESS_LENGTH, MinimumLength = MIN_ADDRESS_LENGTH)]
	public string Address { get; set; } = null!;

	[Required]
	[EmailAddress]
	public string Email { get; set; } = null!;

	[Required]
	[Phone]
	[Display(Name = "Phone Number")]
	public string PhoneNumber { get; set; } = null!;

	[Required]
	public bool IsActive { get; set; }
}