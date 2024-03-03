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
	public Guid Id { get; set; }

	[Required]
	public bool IsWorking { get; set; } = true;

	public ICollection<Subject> Subjects { get; set; }
}