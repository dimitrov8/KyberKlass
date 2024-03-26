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
	[ForeignKey(nameof(ApplicationUser))]
	public Guid Id { get; set; }

	public ApplicationUser ApplicationUser { get; set; } = null!;

	public ICollection<Student> Students { get; set; }
}