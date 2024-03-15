namespace KyberKlass.Services.Data.Interfaces;

using Web.ViewModels.Admin.User;

public interface ITeacherService
{
	Task<List<UserViewModel>?> AllAsync();

	Task<IEnumerable<UserBasicViewModel>> GetUnassignedTeachersAsync();
}