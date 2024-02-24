namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using static Common.EntityValidations.ApplicationUser;

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
	public ICollection<Student> Students { get; set; } 
}