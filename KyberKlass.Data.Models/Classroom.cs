namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Common.EntityValidations.Classroom;

public class Classroom
{
	public Classroom()
	{
		this.Id = Guid.NewGuid();

		this.Students = new HashSet<Student>();
		this.Subjects = new HashSet<Subject>();
		this.Grades = new HashSet<Grade>();
	}

	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public Guid Id { get; set; }

	[Required]
	[MaxLength(MAX_NAME_LENGTH)]
	public string Name { get; set; } = null!;

	[Required]
	public Guid TeacherId { get; set; }

	[ForeignKey(nameof(TeacherId))]
	public Teacher Teacher { get; set; } = null!;

	[Required]
	public bool IsActive { get; set; } = false;

	public ICollection<Student> Students { get; set; }

	public ICollection<Subject> Subjects { get; set; }

	public ICollection<Grade> Grades { get; set; }

	public override string ToString()
	{
		return this.Name;
	}
}