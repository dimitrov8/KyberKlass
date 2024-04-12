namespace KyberKlass.Services.Data.Interfaces;

using KyberKlass.Data.Models;
using Web.ViewModels.Admin;
using Web.ViewModels.Admin.Student;
using Web.ViewModels.Admin.User;

public interface IStudentService
{
	public Task<IEnumerable<UserViewModel>?> AllAsync();

	public Task<Student?> GetByIdASync(string id);

	Task<IEnumerable<UserBasicViewModel>> GetUnassignedStudentsAsync();

	Task<StudentChangeGuardianViewModel> GetStudentChangeGuardianAsync(string userId);

	Task<bool> StudentChangeGuardianAsync(string userId, string guardianId);
}