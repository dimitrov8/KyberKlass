namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Common.EntityValidations.Subject;

public class Grade
{
	[Key]
	public int Id { get; set; }

	[Required]
	public Guid StudentId { get; set; }

	[ForeignKey(nameof(StudentId))]
	public Student Student { get; set; } = null!;

	[Required]
	[StringLength(GRADE_LENGTH)]
	[RegularExpression(GRADE_REGEX)]
	public string Value { get; set; } = null!;

	[Required]
	public int SubjectId { get; set; }

	[ForeignKey(nameof(SubjectId))]
	public Subject Subject { get; set; } = null!;
}