namespace KyberKlass.Services.Data.Interfaces;

using Web.ViewModels.Admin.Classroom;

public interface IClassroomService
{
	Task<ManageClassroomsViewModel> GetManageClassroomsAsync(string schoolId);

	Task<IEnumerable<ClassroomViewModel>> GetClassroomsAsync(string schoolId);

    Task<IEnumerable<ClassroomBasicViewModel>> GetAllSchoolClassroomsAsync();

	Task<bool> AddAsync(AddClassroomViewModel model);
}