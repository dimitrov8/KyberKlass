namespace KyberKlass.Services.Data.Interfaces;

using KyberKlass.Data.Models;
using Web.ViewModels.Admin.School;

public interface ISchoolService
{
	Task<bool> AddAsync(AddSchoolFormModel model);

	Task<IEnumerable<SchoolViewModel>> AllAsync();

	Task<bool> ExistAsync(School newSchool);

	Task<AddSchoolFormModel?> GetForEditAsync(string id);

	Task<SchoolViewModel?> GetForDeleteAsync(string id);

	Task<bool> EditSchoolAsync(string id, SchoolViewModel model);

	Task<SchoolViewModel?> ViewDetailsAsync(string id);

	Task<bool> DeleteAsync(string? id);

	Task<bool> IsNotNullOrEmptyInputAsync(string id, SchoolViewModel? model);

	Task<SchoolViewModel?> GetByIdAsync(string id);
}