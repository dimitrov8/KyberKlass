using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static KyberKlass.Common.EntityValidations.BaseUser;

namespace KyberKlass.Web.ViewModels.Admin.User;
public class UserEditFormModel
{
    [Required]
    public string Id { get; set; } = null!;

    [Required]
    [MaxLength(MAX_NAME_LENGTH)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(MAX_NAME_LENGTH)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = null!;

    [Required]
    [Column(TypeName = "DATE")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Birth Date")]
    public string BirthDate { get; set; } = null!;

    [Required]
    public string Address { get; set; } = null!;

    [Required]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    [Display(Name = "Is Active")]
    public bool IsActive { get; set; } = true;
}