namespace KyberKlass.Web.ViewModels.Admin.Classroom;

using System.ComponentModel.DataAnnotations;
using User;
using static Common.EntityValidations.Classroom;

public class ClassroomCreateFormModel
{
    public ClassroomCreateFormModel()
    {
        this.Students = new HashSet<UserBasicVIewModel>();
        this.UnassignedTeachers = new HashSet<UserBasicVIewModel>();
        this.SelectedStudents = new HashSet<UserBasicVIewModel>();
    }

    [Required(ErrorMessage = "Classroom name is required.")]
    [StringLength(MAX_NAME_LENGTH, MinimumLength = MIN_NAME_LENGTH)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Please select a teacher")]
    public string TeacherId { get; set; } = null!;

    [Required]
    public bool IsActive { get; set; }

    [Required]
    public string SchoolId { get; set; } = null!;

    public ICollection<UserBasicVIewModel> UnassignedTeachers { get; set; }

    public ICollection<UserBasicVIewModel> Students { get; set; }

    public ICollection<UserBasicVIewModel> SelectedStudents { get; set; }
}