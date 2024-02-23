namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
///     Represents the association between a teacher and a subject taught by that teacher.
/// </summary>
public class TeacherSubject
{
	[Required]
	public Guid TeacherId { get; set; }

	[Required]
	[ForeignKey(nameof(TeacherId))]
	public Teacher Teacher { get; set; } = null!;

	[Required]
	public int SubjectId { get; set; }

	[ForeignKey(nameof(SubjectId))]
	public Subject Subject { get; set; } = null!; 
}