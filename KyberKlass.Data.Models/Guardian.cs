namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;

public class Guardian
{
	public Guardian()
	{
		this.Students = new HashSet<Student>();
	}

	[Key]
	public Guid Id { get; set; }

	public ICollection<Student> Students { get; set; }
}