namespace KyberKlass.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using static Common.EntityValidations.BaseUser;
using static Common.FormattingConstants;

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

	public IdentityRole<Guid>? Role { get; set; }

	[Required]
	public bool IsActive { get; set; } = true;

	public string GetFullName()
	{
		return $"{this.FirstName} {this.LastName}";
	}

	public string GetBirthDate()
	{
		return $"{this.BirthDate.ToString(BIRTH_DATE_FORMAT, CultureInfo.InvariantCulture)}";
	}

	public string GetStatus()
	{
		return this.IsActive ? "True" : "False";
	}

	public async Task<string> GetRoleAsync(UserManager<ApplicationUser> userManager)
	{
		IList<string>? userRoles = await userManager.GetRolesAsync(this);
		return userRoles.FirstOrDefault() ?? "No Role Assigned";
	}
}