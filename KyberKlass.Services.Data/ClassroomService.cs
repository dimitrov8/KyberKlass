namespace KyberKlass.Services.Data;

using Interfaces;
using KyberKlass.Data;
using KyberKlass.Data.Models;
using KyberKlass.Web.ViewModels.Admin;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.Admin.Classroom;
using Web.ViewModels.Admin.User;

/// <summary>
///     Service class responsible for managing classrooms.
/// </summary>
public class ClassroomService : IClassroomService
{
	private readonly KyberKlassDbContext _dbContext;

	/// <summary>
	///     Constructor for ClassroomService.
	/// </summary>
	/// <param name="dbContext">The database context.</param>
	public ClassroomService(KyberKlassDbContext dbContext)
	{
		this._dbContext = dbContext;
	}

	/// <summary>
	///     Retrieves a view model containing classrooms for a specified school.
	/// </summary>
	/// <param name="schoolId">The ID of the school.</param>
	/// <returns>A view model containing classrooms for the school.</returns>
	public async Task<ManageClassroomsViewModel> GetManageClassroomsAsync(string schoolId)
	{
		var school = await this._dbContext.Schools.FindAsync(Guid.Parse(schoolId)); // Retrieve the school
		IEnumerable<ClassroomViewModel> classrooms = await this._dbContext
			.Classrooms
			.Where(c => c.SchoolId == school!.Id)
			.Select(c => new ClassroomViewModel
			{
				Id = c.Id.ToString(),
				Name = c.Name,
				TeacherName = c.Teacher.ApplicationUser.GetFullName(),
				Students = c.Students
					.Select(s => new BasicViewModel
					{
						Id = s.Id.ToString(),
						Name = s.ApplicationUser.GetFullName()
					})
					.ToArray()
			})
			.AsNoTracking()
			.ToArrayAsync();

		return new ManageClassroomsViewModel
		{
			SchoolId = schoolId,
			SchoolName = school!.Name,
			Classrooms = classrooms
		}; // Create and return a view model containing classrooms
	}


	public async Task<IEnumerable<ClassroomViewModel>> GetClassroomsAsync(string schoolId)
	{
		Classroom[] classrooms = await this._dbContext
			.Classrooms
			.Where(c => c.SchoolId == Guid.Parse(schoolId))
			.Include(c => c.Students)
			.ThenInclude(s => s.ApplicationUser)
			.Include(u => u.Teacher.ApplicationUser)
			.AsNoTracking()
			.ToArrayAsync();

		IEnumerable<ClassroomViewModel> classroomViewModels = classrooms.Select(c => new ClassroomViewModel
		{
			Id = c.Id.ToString(),
			Name = c.Name,
			TeacherName = c.Teacher.ApplicationUser.GetFullName(),
			Students = c.Students // todo classrooms
				.Select(s => new BasicViewModel
				{
					Id = s.Id.ToString(),
					Name = s.ApplicationUser.GetFullName()
				})
				.ToArray()
		});

		return classroomViewModels;
	}

	public async Task<IEnumerable<BasicViewModel>> GetAllSchoolClassroomsAsync()
	{
		IEnumerable<BasicViewModel> allClassrooms = await this._dbContext
			.Classrooms
			.Select(c => new BasicViewModel
			{
				Id = c.Id.ToString(),
				Name = c.Name
			})
			.AsNoTracking()
			.ToArrayAsync();

		return allClassrooms;
	}

	/// <summary>
	///     Adds a new classroom to the database.
	/// </summary>
	/// <param name="model">The view model containing data for the new classroom.</param>
	/// <returns>True if the classroom was added successfully; otherwise, false.</returns>
	public async Task<bool> AddAsync(AddClassroomViewModel model)
	{
		var newSchool = await this._dbContext.Schools.FindAsync(Guid.Parse(model.SchoolId)); // Retrieve the school by ID

		if (newSchool == null)
		{
			return false; // If the school doesn't exist, return false
		}

		//bool classRoomExists = await this.ClassroomExistsInSchool(model.Name, model.SchoolId); // Check if the classroom already exists in the school

		//if (classRoomExists)
		//{
		//	return false; // If the classroom already exists, return false
		//}

		var schoolIdAsGuid = Guid.Parse(model.SchoolId);
		var teacherIdAsGuid = Guid.Parse(model.TeacherId);

		var newClassroom = new Classroom
		{
			Id = Guid.NewGuid(),
			Name = model.Name,
			TeacherId = teacherIdAsGuid,
			SchoolId = schoolIdAsGuid
		}; // Create a new classroom object

		foreach (string studentId in model.SelectedStudents.Select(s => s.Id))
		{
			var student = await this._dbContext
				.Students
				.FindAsync(Guid.Parse(studentId));

			if (student != null)
			{
				newClassroom.Students.Add(student);
			}
		}

		// Add the new classroom to the database
		await this._dbContext.Classrooms.AddAsync(newClassroom);
		await this._dbContext.SaveChangesAsync();

		return true;
	}

	/// <summary>
	///     Checks if a classroom with the given name exists in the specified school.
	/// </summary>
	/// <param name="classroomName">The name of the classroom.</param>
	/// <param name="schoolId">The ID of the school.</param>
	/// <returns>True if the classroom exists in the school; otherwise, false.</returns>
	public async Task<bool> ClassroomExistsInSchoolAsync(string classroomName, string schoolId)
	{
		return await this._dbContext
			.Classrooms
			.AnyAsync(c => c != null && c.Name == classroomName && c.SchoolId == Guid.Parse(schoolId));
		// Check if any classroom with the given name and school ID exists in the database
	}

	public  async Task<bool> DeleteAsync(string classroomId)
	{
		var classroomToDelete = await this._dbContext.Classrooms.FindAsync(Guid.Parse(classroomId));

		// If the classroom exists and there are no students in this classrooms
		if (classroomToDelete != null && classroomToDelete.Students.Any() == false)
		{
			// Remove the classroom from the database
			this._dbContext.Classrooms.Remove(classroomToDelete);
			await this._dbContext.SaveChangesAsync();

			return true; // Return true if the classroom is successfully deleted
		}

		return false; // Return false if the classroom to delete is not found
	}

	public async Task<BasicViewModel?> GetClassroomAsync(string id)
	{
		return await this._dbContext
			.Classrooms
			.Select(c => new BasicViewModel
			{
				Id = c!.Id.ToString(),
				Name = c.Name
			})
			.FirstOrDefaultAsync(c => c.Id == id);
	}
}