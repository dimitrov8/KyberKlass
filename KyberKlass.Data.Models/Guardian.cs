namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Guardian
{
	public Guardian()
	{
		this.Students = new HashSet<Student>();
	}

	[Key]
	public Guid Id { get; set; }

	public ICollection<Student> Students { get; set; }

	[ForeignKey(nameof(Id))]
	public ApplicationUser User { get; set; } = null!;

	[Required]
	public bool IsActive { get; set; } = true;
}