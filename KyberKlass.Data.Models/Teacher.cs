namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Teacher : BasePerson
{
	public Teacher()
	{
		this.Subjects = new HashSet<Subject>();
	}

	[Required]
	public bool IsWorking { get; set; } = true;

	public ICollection<Subject> Subjects { get; set; }

	public override string ToString()
	{
		return this.Id.ToString();
	}
}