namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Common.EntityValidations.Praise;

/// <summary>
///     Represents a praise given to a student by a teacher for exemplary performance.
/// </summary>
public class Praise
{
	[Required]
	public Guid StudentId { get; set; }

	[ForeignKey(nameof(StudentId))]
	public Student Student { get; set; } = null!;

	[Required]
	public Guid TeacherId { get; set; }

	[ForeignKey(nameof(TeacherId))]
	public Teacher Teacher { get; set; } = null!;

	[Required]
	public Guid ClassroomId { get; set; }

	[ForeignKey(nameof(ClassroomId))]
	public Classroom Classroom { get; set; } = null!;

	[Required]
	public DateTime Date { get; set; }

	[Required]
	[MaxLength(MAX_DESCRIPTION_LENGTH)]
	public string Description { get; set; } = null!;
}