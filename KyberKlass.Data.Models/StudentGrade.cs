namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Common.EntityValidations.Student;

/// <summary>
///     Represents a grade assigned to a student for a particular subject.
/// </summary>
public class StudentGrade
{
	[Required]
	[Range(MIN_GRADE_MARK, MAX_GRADE_MARK)]
	public double Grade { get; set; }

	[Required]
	public Guid StudentId { get; set; }

	[ForeignKey(nameof(StudentId))]
	public Student Student { get; set; } = null!;

	[Required]
	public int SubjectId { get; set; }

	[ForeignKey(nameof(SubjectId))]
	public Subject Subject { get; set; } = null!;
}