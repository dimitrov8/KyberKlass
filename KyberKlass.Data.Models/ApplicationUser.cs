namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using static Common.EntityValidations.ApplicationUser;

public class ApplicationUser : IdentityUser<Guid>
{
	[Required]
	[MaxLength(MAX_NAME_LENGTH)]
	public string FirstName { get; set; } = null!;

	[Required]
	[MaxLength(MAX_NAME_LENGTH)]
	public string LastName { get; set; } = null!;

	[Required]
	public DateTime BirthDate { get; set; }

	[Required]
	public string Address { get; set; } = null!;

	[Required]
	[EmailAddress]
	public override string Email { get; set; } = null!;

	[Required]
	[Phone]
	public override string PhoneNumber { get; set; } = null!;

	[Required]
	public UserRole Role { get; set; } 
}

public enum UserRole
{
	Student,
	Teacher,
	Admin,
	Guardian
}