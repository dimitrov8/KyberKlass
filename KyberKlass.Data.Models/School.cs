namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using static Common.EntityValidations.School;

public class School
{
	public School()
	{
		this.Classrooms = new HashSet<Classroom>();
	}

	[Key]
	public Guid Id { get; set; }

	[Required]
	[MaxLength(MAX_NAME_LENGTH)]
	public string Name { get; set; } = null!;

	[Required]
	public string Address { get; set; } = null!;

	[Required]
	[EmailAddress]
	public string Email { get; set; } = null!;

	[Required]
	[Phone]
	public string Phone { get; set; } = null!;

	public ICollection<Classroom> Classrooms { get; set; }
}