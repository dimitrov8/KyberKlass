namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using static Common.EntityValidations.Classroom;

/// <summary>
///     Represents a classroom in the school.
/// </summary>
public class Classroom
{
	public Classroom()
	{
		this.Id = Guid.NewGuid();
		this.Students = new HashSet<Student>();
		this.Subjects = new HashSet<Subject>();
		this.Grades = new HashSet<StudentGrade>();
		this.Assignments = new HashSet<Assignment>();
		this.Exams = new HashSet<Exam>();
		this.Absences = new HashSet<Absence>();
		this.Behaviors = new HashSet<Behavior>();
		this.Praises = new HashSet<Praise>();
	}

	[Key]
	public Guid Id { get; set; }

	[Required]
	[MaxLength(MAX_NAME_LENGTH)]
	public string Name { get; set; } = null!;

	public ICollection<Student> Students { get; set; }

	public ICollection<Subject> Subjects { get; set; }

	public ICollection<StudentGrade> Grades { get; set; }

	public ICollection<Assignment> Assignments { get; set; }

	public ICollection<Exam> Exams { get; set; }

	public ICollection<Absence> Absences { get; set; }

	public ICollection<Behavior> Behaviors { get; set; }

	public ICollection<Praise> Praises { get; set; }
}