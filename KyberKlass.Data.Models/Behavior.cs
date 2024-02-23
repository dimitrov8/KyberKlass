namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Common.EntityValidations.Behavior;

/// <summary>
///     Represents a unacceptable behavior for a student.
/// </summary>
public class Behavior
{
	[Required]
	public Guid StudentId { get; set; }

	[ForeignKey(nameof(StudentId))]
	public Student Student { get; set; } = null!;

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