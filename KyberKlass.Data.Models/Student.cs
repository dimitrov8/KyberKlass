namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Common.EntityValidations.Student;

/// <summary>
///     Represents a student in the school.
/// </summary>
public class Student
{
	public Student()
	{
		this.Id = Guid.NewGuid();
	}

	[Key]
	public Guid Id { get; set; }

	[Required]
	[Range(MIN_GRADE_LEVEL, MAX_GRADE_LEVEL)]
	public int GradeLevel { get; set; }

	[Required]
	public Guid GuardianId { get; set; }

	[ForeignKey(nameof(GuardianId))]
	public Guardian Guardian { get; set; } = null!;

	[Required]
	public Guid SchoolId { get; set; }

	[ForeignKey(nameof(SchoolId))]
	public School School { get; set; } = null!;

	[Required]
	public Guid ClassroomId { get; set; }

	[ForeignKey(nameof(ClassroomId))]
	public Classroom Classroom { get; set; } = null!;

	[Required]
	public bool IsEnrolled { get; set; } = true;
}