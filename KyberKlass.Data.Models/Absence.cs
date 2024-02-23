namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
///     Represents an absence record for a student in the school.
/// </summary>
public class Absence
{
	[Required]
	public Guid StudentId { get; set; }

	[ForeignKey(nameof(StudentId))]
	public Student Student { get; set; } = null!;

	[Required]
	public DateTime Date { get; set; }

	[Required]
	public bool IsExcused { get; set; }
}