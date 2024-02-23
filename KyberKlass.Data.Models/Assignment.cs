namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Common.EntityValidations.Assignment;

/// <summary>
///     Represents an assignment given to a classroom.
/// </summary>
public class Assignment
{
	[Required]
	public Guid ClassroomId { get; set; }

	[ForeignKey(nameof(ClassroomId))]
	public Classroom Classroom { get; set; } = null!;

	[Required]
	[MaxLength(MAX_TITLE_LENGTH)]
	public string Title { get; set; } = null!;

	[MaxLength(MAX_DESCRIPTION_LENGTH)]
	public string Description { get; set; } = null!;

	[Required]
	public DateTime DueDate { get; set; }
}