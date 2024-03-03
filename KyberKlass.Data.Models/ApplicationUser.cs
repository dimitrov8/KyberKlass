namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using static Common.EntityValidations.BaseUser;

public class ApplicationUser : IdentityUser<Guid>
{
    [Required]
    [MaxLength(MAX_NAME_LENGTH)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(MAX_NAME_LENGTH)]
    public string LastName { get; set; } = null!;

    [Required]
    public string BirthDate { get; set; } = DateTime.UtcNow.ToString("yy-MM-dd", CultureInfo.InvariantCulture);

    [Required]
    public string Address { get; set; } = null!;
}