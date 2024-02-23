namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using static Common.EntityValidations.Person;

/// <summary>
///     Represents a guardian for a student/s in the school.
/// </summary>
public class Guardian
{
	public Guardian()
	{
		this.Id = Guid.NewGuid();
		this.Students = new HashSet<Student>();
	}

	[Key]
	public Guid Id { get; set; }

	[Required]
	[MaxLength(MAX_NAME_LENGTH)]
	public string FirstName { get; set; } = null!;

	[Required]
	[MaxLength(MAX_NAME_LENGTH)]
	public string LastName { get; set; } = null!;

	[Required]
	[EmailAddress]
	public string Email { get; set; } = null!;

	[Required]
	[Phone]
	public string PhoneNumber { get; set; } = null!;

	[Required]
	public ICollection<Student> Students { get; set; }
}