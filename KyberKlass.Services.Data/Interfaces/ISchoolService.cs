namespace KyberKlass.Services.Data.Interfaces;

using KyberKlass.Data.Models;
using Web.ViewModels.Admin.School;

public interface ISchoolService
{
	Task<bool> AddSchoolAsync(AddSchoolFormModel model);

	Task<IEnumerable<SchoolViewModel>> AllSchoolsAsync();

	Task<bool> SchoolExistAsync(School newSchool);

	//Task<bool> RemoveSchoolAsync();
	Task<SchoolViewModel?> GetForEditSchoolAsync(string id);

	Task EditSchoolAsync(string id, SchoolViewModel model);
}