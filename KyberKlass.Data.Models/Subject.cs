namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using static Common.EntityValidations.Subject;

/// <summary>
///     Represents a subject in the school.
/// </summary>
public class Subject
{
	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(MAX_NAME_LENGTH)]
	public string Name { get; set; } = null!; 
}