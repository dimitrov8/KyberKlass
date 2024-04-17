namespace KyberKlass.Services.Data.Interfaces;

using KyberKlass.Data.Models;
using Web.ViewModels.Admin.Student;
using Web.ViewModels.Admin.User;

public interface IStudentService
{
    Task<IEnumerable<UserViewModel>?> AllAsync();

    Task<Student?> GetByIdASync(string id);

    Task<StudentChangeGuardianViewModel> GetStudentChangeGuardianAsync(string userId);

    Task<bool> StudentChangeGuardianAsync(string userId, string guardianId);
}