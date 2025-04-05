using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KyberKlass.Data.Models;
public class Guardian
{
    public Guardian()
    {
        Students = new HashSet<Student>();
    }

    [Key]
    [ForeignKey(nameof(ApplicationUser))]
    public Guid Id { get; set; }

    public ApplicationUser ApplicationUser { get; set; } = null!;

    public ICollection<Student> Students { get; set; }
}