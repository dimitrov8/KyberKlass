using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using static KyberKlass.Common.EntityValidations.BaseUser;
using static KyberKlass.Common.FormattingConstants;

namespace KyberKlass.Data.Models;
public class ApplicationUser : IdentityUser<Guid>
{
    [Required]
    [MaxLength(MAX_NAME_LENGTH)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(MAX_NAME_LENGTH)]
    public string LastName { get; set; } = null!;

    [Required]
    [Column(TypeName = "DATE")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime BirthDate { get; set; }

    [Required]
    public string Address { get; set; } = null!;

    public Guid? RoleId { get; set; }

    [ForeignKey(nameof(RoleId))]
    public IdentityRole<Guid>? Role { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }

    public string GetBirthDate()
    {
        return $"{BirthDate.ToString(BIRTH_DATE_FORMAT, CultureInfo.InvariantCulture)}";
    }

    public string GetStatus()
    {
        return IsActive ? "True" : "False";
    }

    public async Task<string> GetRoleAsync(UserManager<ApplicationUser> userManager)
    {
        IList<string>? userRoles = await userManager.GetRolesAsync(this);
        return userRoles.FirstOrDefault() ?? "No Role Assigned";
    }
}