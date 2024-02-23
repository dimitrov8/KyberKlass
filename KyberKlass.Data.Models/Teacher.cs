namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using static Common.EntityValidations.Person;

/// <summary>
///     Represents a teacher in the school.
/// </summary>
public class Teacher
{
	public Teacher()
	{
		this.Id = Guid.NewGuid();
		this.SubjectsTaught = new HashSet<TeacherSubject>();
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
	public DateTime BirthDate { get; set; }

	[Required]
	public string Address { get; set; } = null!;

	[Required]
	[EmailAddress]
	public string Email { get; set; } = null!;

	[Required]
	[Phone]
	public string PhoneNumber { get; set; } = null!;

	public ICollection<TeacherSubject> SubjectsTaught { get; set; } 
}