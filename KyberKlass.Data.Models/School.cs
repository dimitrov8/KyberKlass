using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static KyberKlass.Common.EntityValidations.School;

namespace KyberKlass.Data.Models;
public class School
{
    public School()
    {
        Id = Guid.NewGuid();
        Classrooms = new HashSet<Classroom>();
        Students = new HashSet<Student>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(MAX_NAME_LENGTH)]
    public string Name { get; set; } = null!;

    [Required]
    public string Address { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    public bool IsActive { get; set; } = true;

    public ICollection<Classroom> Classrooms { get; set; }

    public ICollection<Student> Students { get; set; }
}