namespace KyberKlass.Data.Models;

public class Guardian : BasePerson
{
	public Guardian()
	{
		this.Students = new HashSet<Student>();
	}

	public ICollection<Student> Students { get; set; }
}