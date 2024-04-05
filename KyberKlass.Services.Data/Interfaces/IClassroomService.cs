namespace KyberKlass.Services.Data.Interfaces;

using KyberKlass.Data.Models;
using KyberKlass.Web.ViewModels.Admin;
using Web.ViewModels.Admin.Classroom;

public interface IClassroomService
{
	Task<BasicViewModel?> GetClassroomAsync(string id);

	Task<ManageClassroomsViewModel> GetManageClassroomsAsync(string schoolId);

	Task<IEnumerable<ClassroomViewModel>> GetClassroomsAsync(string schoolId);

	Task<IEnumerable<BasicViewModel>> GetAllSchoolClassroomsAsync();

	Task<bool> AddAsync(AddClassroomViewModel model);

	Task<bool> ClassroomExistsInSchoolAsync(string classroomName, string schoolId);

	//Task<bool> ClassroomExistsAsync(string classroomId);

	//Task<bool> DeleteAsync(string schoolId, string classroomId);
}