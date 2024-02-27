namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Grade
{
	[Key]
	public int Id { get; set; }

	[Required]
	public Guid StudentId { get; set; }

	[ForeignKey(nameof(StudentId))]
	public Student Student { get; set; } = null!;

	[Required]
	// TODO ADD VALIDATION
	public double Value { get; set; }

	[Required]
	public int SubjectId { get; set; }

	[ForeignKey(nameof(SubjectId))]
	public Subject Subject { get; set; } = null!;
}