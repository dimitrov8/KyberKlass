namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;

/// <summary>
///     Represents a teacher in the school.
/// </summary>
public class Teacher
{
	public Teacher()
	{
		this.Id = Guid.NewGuid();
		this.TeachingSubjects = new HashSet<TeacherSubject>();
	}

	[Key]
	public Guid Id { get; set; }

	[Required]
	public bool IsWorking { get; set; } = true;

	public ICollection<TeacherSubject> TeachingSubjects { get; set; }
}