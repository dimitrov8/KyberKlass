namespace KyberKlass.Services.Data;

using Interfaces;
using KyberKlass.Data;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.Admin.Classroom;

public class ClassroomService : IClassroomService
{
	private readonly KyberKlassDbContext _dbContext;
	private readonly ISchoolService _schoolService;

	public ClassroomService(KyberKlassDbContext dbContext, ISchoolService schoolService)
	{
		this._dbContext = dbContext;
		this._schoolService = schoolService;
	}

	public async Task<ManageClassroomsViewModel> GetManageClassroomsAsync(string schoolId)
	{
		var school = await this._schoolService.GetByIdAsync(schoolId);
		IEnumerable<ClassroomViewModel> classrooms = await this.GetClassroomsAsync(schoolId);

		return new ManageClassroomsViewModel
		{
			SchoolId = schoolId,
			SchoolName = school!.Name,
			Classrooms = classrooms
				.Select(c => new ClassroomViewModel
				{
					Id = c.Id,
					Name = c.Name
				})
		};
	}

	public async Task<IEnumerable<ClassroomViewModel>> GetClassroomsAsync(string schoolId)
	{
		IEnumerable<ClassroomViewModel> classrooms = await this._dbContext
			.Classrooms
			.Where(c => c.SchoolId.ToString() == schoolId)
			.Select(c => new ClassroomViewModel
			{
				Id = c.Id.ToString(),
				Name = c.Name
			})
			.AsNoTracking()
			.ToArrayAsync();

		return classrooms;
	}
}