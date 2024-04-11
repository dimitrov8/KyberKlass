namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Student
{
	public Student()
	{
		this.Grades = new HashSet<Grade>();
		this.Absences = new HashSet<Absence>();
	}

	[Key]
	[ForeignKey(nameof(ApplicationUser))]
	public Guid Id { get; set; }

	public ApplicationUser ApplicationUser { get; set; } = null!;

	[Required]
	public Guid GuardianId { get; set; }

	[ForeignKey(nameof(GuardianId))]
	public Guardian Guardian { get; set; } = null!;

	[Required]
	public Guid ClassroomId { get; set; }

	[ForeignKey(nameof(ClassroomId))]
	public Classroom Classroom { get; set; } = null!;

	public ICollection<Grade> Grades { get; set; }

	public ICollection<Absence> Absences { get; set; }
}