namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Student
{
	[Key]
	[ForeignKey(nameof(ApplicationUser))]
	public Guid Id { get; set; }

	public ApplicationUser ApplicationUser { get; set; } = null!;

	[Required]
	public Guid GuardianId { get; set; }

	[ForeignKey(nameof(GuardianId))]
	public Guardian Guardian { get; set; } = null!;

	[Required]
	public Guid SchoolId { get; set; }

    [ForeignKey(nameof(SchoolId))]
    public School School { get; set; } = null!;

	[Required]
	public Guid ClassroomId { get; set; }

	[ForeignKey(nameof(ClassroomId))]
	public Classroom Classroom { get; set; } = null!;
}