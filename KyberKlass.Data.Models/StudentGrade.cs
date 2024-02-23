namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Common.EntityValidations.Student;

public class StudentGrade
{
	[Key]
	public int Id { get; set; }

	[Required]
	[Range(MIN_GRADE_MARK, MAX_GRADE_MARK)]
	public double Grade { get; set; }

	[Required]
	public Guid StudentId { get; set; }

	[ForeignKey(nameof(StudentId))]
	public Student Student { get; set; } = null!;
}