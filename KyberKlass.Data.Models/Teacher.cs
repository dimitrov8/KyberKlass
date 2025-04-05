using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KyberKlass.Data.Models;
public class Teacher
{
    [Key]
    [ForeignKey(nameof(ApplicationUser))]
    public Guid Id { get; set; }

    public ApplicationUser ApplicationUser { get; set; } = null!;
}