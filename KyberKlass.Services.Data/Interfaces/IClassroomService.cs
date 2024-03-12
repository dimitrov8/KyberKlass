namespace KyberKlass.Services.Data.Interfaces
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Web.ViewModels.Admin.Classroom;

	public interface IClassroomService
	{
		Task<ManageClassroomsViewModel> GetManageClassroomsAsync(string schoolId);

		Task<IEnumerable<ClassroomViewModel>> GetClassroomsAsync(string schoolId);
	}
}
