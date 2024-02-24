namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Common.EntityValidations.Exam;

/// <summary>
///     Represents an exam conducted in a classroom for a specific subject.
/// </summary>
public class Exam
{
	[Required]
	public Guid ClassroomId { get; set; }

	[ForeignKey(nameof(ClassroomId))]
	public Classroom Classroom { get; set; } = null!;

	[Required]
	public int SubjectId { get; set; }

	[ForeignKey(nameof(SubjectId))]
	public Subject Subject { get; set; } = null!;

	[Required]
	public DateTime Date { get; set; }

	[MaxLength(MAX_DESCRIPTION_LENGTH)]
	public string? Description { get; set; }
}