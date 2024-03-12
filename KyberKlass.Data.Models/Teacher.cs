namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;

public class Teacher
{
	public Teacher()
	{
		this.Subjects = new HashSet<Subject>();
	}

	[Key]
	public Guid Id { get; set; }

	[Required]
	public bool IsActive { get; set; } = true;

	public ICollection<Subject> Subjects { get; set; }
}