namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using static Common.EntityValidations.ApplicationUser;

public class ApplicationUser : IdentityUser<Guid>
{
	[MaxLength(MAX_NAME_LENGTH)]
	public string? FirstName { get; set; }

	[MaxLength(MAX_NAME_LENGTH)]
	public string? LastName { get; set; }

	public DateTime? BirthDate { get; set; }

	public string? Address { get; set; }

	[EmailAddress]
	public override string Email { get; set; } = null!;

	[Phone]
	public override string? PhoneNumber { get; set; }
}