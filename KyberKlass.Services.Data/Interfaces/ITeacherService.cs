namespace KyberKlass.Services.Data.Interfaces;

using KyberKlass.Web.ViewModels.Admin;
using Web.ViewModels.Admin.User;

public interface ITeacherService
{
	Task<List<UserViewModel>?> AllAsync();

	Task<IEnumerable<BasicViewModel>> GetUnassignedTeachersAsync();
}