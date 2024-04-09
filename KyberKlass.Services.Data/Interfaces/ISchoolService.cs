namespace KyberKlass.Services.Data.Interfaces;

using Web.ViewModels.Admin;
using Web.ViewModels.Admin.School;

public interface ISchoolService
{
	Task<bool> AddAsync(AddSchoolFormModel model);

	Task<IEnumerable<SchoolViewModel>> AllAsync();

    Task<IEnumerable<BasicViewModel>> GetSchoolsAsync();

	Task<AddSchoolFormModel?> GetForEditAsync(string id);

	Task<SchoolViewModel?> GetForDeleteAsync(string id);

	Task<bool> EditAsync(string id, SchoolViewModel model);

	Task<SchoolViewModel?> ViewDetailsAsync(string id);

	Task<bool> DeleteAsync(string id);

	Task<SchoolViewModel?> GetByIdAsync(string id);

	//Task<IEnumerable<UserBasicViewModel>> GetTeachersAsync();
}