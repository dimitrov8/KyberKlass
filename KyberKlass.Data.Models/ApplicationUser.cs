namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using static Common.EntityValidations.BaseUser;

public class ApplicationUser : IdentityUser<Guid>
{
	[Required]
	[MaxLength(MAX_NAME_LENGTH)]
	public string FirstName { get; set; } = null!;

	[Required]
	[MaxLength(MAX_NAME_LENGTH)]
	public string LastName { get; set; } = null!;

	[Required]
	[Column(TypeName = "DATE")]
	[DataType(DataType.Date)]
	[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
	public DateTime BirthDate { get; set; }

	[Required]
	public string Address { get; set; } = null!;

	[Required]
	public bool IsActive { get; set; } = true;
}