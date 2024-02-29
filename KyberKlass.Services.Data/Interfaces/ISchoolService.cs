namespace KyberKlass.Services.Data.Interfaces;

using KyberKlass.Data.Models;
using Web.ViewModels.Admin.School;

public interface ISchoolService
{
	Task<bool> AddAsync(AddSchoolFormModel model);

	Task<IEnumerable<SchoolViewModel>> AllAsync();

	Task<bool> ExistAsync(School newSchool);

	//Task<bool> RemoveSchoolAsync();
	Task<AddSchoolFormModel?> GetForEditAsync(string id);

	Task<bool> EditSchoolAsync(string id, SchoolViewModel model);

	Task<SchoolViewModel?> ViewDetailsAsync(string id);
}