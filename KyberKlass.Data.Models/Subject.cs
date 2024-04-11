namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Common.EntityValidations.Subject;

public class Subject
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	[Required]
	[MaxLength(MAX_NAME_LENGTH)]
	public string Name { get; set; } = null!;

	[Required]
	public Guid TeacherId { get; set; }

	[ForeignKey(nameof(TeacherId))]
	public Teacher Teacher { get; set; } = null!;

	[Required]
	public Guid ClassroomId { get; set; }

	[ForeignKey(nameof(ClassroomId))]
	public Classroom Classroom { get; set; } = null!;
}