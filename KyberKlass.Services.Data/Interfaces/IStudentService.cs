namespace KyberKlass.Services.Data.Interfaces;

using Web.ViewModels.Admin.User;

public interface IStudentService
{
    Task<IEnumerable<UserBasicViewModel>?> GetUnassignedStudentsAsync();
}