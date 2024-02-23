namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using static Common.EntityValidations.Person;
using static Common.EntityValidations.Student;


public class Student
{
	public Student()
	{
		this.Id   = Guid.NewGuid();
		this.Grades = new HashSet<StudentGrade>();
	}

	[Key]
	public Guid Id { get; set; }

	[Required]
	[MaxLength(MAX_NAME_LENGTH)]
	public string FirstName { get; set; } = null!;

	[Required]
	[MaxLength(MAX_NAME_LENGTH)]
	public string LastName { get; set; } = null!;

	[Required]
	public DateTime BirthDate { get; set; }

	[Required]
	[Range(MIN_GRADE_LEVEL, MAX_GRADE_LEVEL)]
	public int GradeLevel { get; set; } 

	[Required]
	public string Address { get; set; } = null!;

	[Required]
	[EmailAddress]
	public string Email { get; set; } = null!;

	[Required]
	[Phone]
	public string PhoneNumber { get; set; } = null!;

	public ICollection<StudentGrade> Grades { get; set; }
}