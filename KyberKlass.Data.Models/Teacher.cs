namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Teacher
{
	public Teacher()
	{
		this.Subjects = new HashSet<Subject>();
	}

	[Key]
	[ForeignKey(nameof(ApplicationUser))]
	public Guid Id { get; set; }

	public ApplicationUser ApplicationUser { get; set; } = null!;

	public ICollection<Subject> Subjects { get; set; }
}