namespace KyberKlass.Services.Data;

using Interfaces;
using KyberKlass.Data;
using KyberKlass.Data.Models;
using KyberKlass.Web.ViewModels.Admin;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.Admin.Classroom;
using Web.ViewModels.Admin.School;
using Web.ViewModels.Admin.User;

/// <summary>
///     Provides functionality to interact with school data in the database.
/// </summary>
public class SchoolService : ISchoolService
{
	private readonly KyberKlassDbContext _dbContext;

	/// <summary>
	///     Initializes a new instance of the <see cref="SchoolService" /> class.
	/// </summary>
	/// <param name="dbContext">The database context.</param>
	public SchoolService(KyberKlassDbContext dbContext)
	{
		this._dbContext = dbContext;
	}

	/// <summary>
	///     Adds a new school to the database.
	/// </summary>
	/// <param name="model">The model containing school data.</param>
	/// <returns>True if the school is successfully added; otherwise, false.</returns>
	public async Task<bool> AddAsync(AddSchoolFormModel model)
	{
		var newSchool = new School
		{
			Id = Guid.NewGuid(),
			Name = model.Name,
			Address = model.Address,
			Email = model.Email,
			PhoneNumber = model.PhoneNumber,
			IsActive = true
		}; // Create a new School object with data from the provided model

		// Check if a school with the same name already exists
		bool schoolExists = await this._dbContext.Schools.AnyAsync(s => s.Name == model.Name);

		if (schoolExists)
		{
			return false;
		}

		// Add the new school to the database and save changes
		await this._dbContext.Schools.AddAsync(newSchool);
		await this._dbContext.SaveChangesAsync(); // Return true if the school is successfully added

		return true;
	}

	/// <summary>
	///     Retrieves all schools from the database.
	/// </summary>
	/// <returns>An enumerable collection of school view models.</returns>
	public async Task<IEnumerable<SchoolViewModel>> AllAsync()
	{
		var schools = await this._dbContext
			.Schools
			.Include(s => s.Classrooms)
			.ThenInclude(c => c.Teacher.ApplicationUser)
			.Include(s => s.Classrooms)
			.ThenInclude(c => c.Students)
			.AsNoTracking()
			.ToArrayAsync();

		var viewModel = schools
			.Select(s => new SchoolViewModel
			{
				Id = s.Id.ToString(),
				Name = s.Name,
				Address = s.Address,
				Email = s.Email,
				PhoneNumber = s.PhoneNumber,
				IsActive = s.IsActive,
				Classrooms = s.Classrooms
					.Select(c => new ClassroomViewModel
					{
						Id = c.Id.ToString(),
						Name = c.Name,
						TeacherName = c.Teacher.ApplicationUser.GetFullName(),
						Students = c.Students
							.Select(st => new BasicViewModel
							{
								Id = st.Id.ToString(),
								Name = st.ApplicationUser.GetFullName()
							})
							.ToArray()
					})
			});

		return viewModel; // Return the collection of schools with their classrooms
	}

	/// <summary>
	///     Retrieves school data for editing based on the provided school ID.
	/// </summary>
	/// <param name="id">The ID of the school to retrieve.</param>
	/// <returns>The school data for editing as an instance of <see cref="AddSchoolFormModel" />.</returns>
	public async Task<AddSchoolFormModel?> GetForEditAsync(string id)
	{
		var viewModel = await this._dbContext
			.Schools
			.Where(s => s.Id == Guid.Parse(id))
			.Select(s => new AddSchoolFormModel
			{
				Name = s.Name,
				Address = s.Address,
				Email = s.Email,
				PhoneNumber = s.PhoneNumber,
				IsActive = s.IsActive
			})
			.AsNoTracking()
			.FirstOrDefaultAsync();

		return viewModel;
	}

	/// <summary>
	///     Retrieves school data for deletion based on the provided school ID.
	/// </summary>
	/// <param name="id">The ID of the school to retrieve.</param>
	/// <returns>The school data for deletion as an instance of <see cref="SchoolViewModel" />.</returns>
	public async Task<SchoolViewModel?> GetForDeleteAsync(string id)
	{
		var viewModel = await this._dbContext
			.Schools
			.Where(s => s.Id == Guid.Parse(id))
			.Select(s => new SchoolViewModel
			{
				Id = s.Id.ToString(),
				Name = s.Name,
				Address = s.Address,
				Email = s.Email,
				PhoneNumber = s.PhoneNumber,
				IsActive = s.IsActive
			})
			.AsNoTracking()
			.FirstOrDefaultAsync();

		return viewModel;
	}

	/// <summary>
	///     Edits an existing school in the database.
	/// </summary>
	/// <param name="id">The ID of the school to edit.</param>
	/// <param name="model">The updated school data.</param>
	/// <returns>True if the school is successfully edited; otherwise, false.</returns>
	public async Task<bool> EditAsync(string id, SchoolViewModel model)
	{
		var schoolForEdit = await this._dbContext.Schools.FindAsync(Guid.Parse(id));

		if (schoolForEdit == null)
		{
			return false; // Return false if the school to edit is not found
		}

		// Update school properties with data from the provided model
		schoolForEdit.Name = model.Name;
		schoolForEdit.Address = model.Address;
		schoolForEdit.Email = model.Email;
		schoolForEdit.PhoneNumber = model.PhoneNumber;
		schoolForEdit.IsActive = model.IsActive;

		await this._dbContext.SaveChangesAsync(); // Save changes to the database

		return true; // Return true if the school is successfully edited
	}

	/// <summary>
	///     Retrieves details of a school based on the provided school ID.
	/// </summary>
	/// <param name="id">The ID of the school to retrieve.</param>
	/// <returns>The details of the school as an instance of <see cref="SchoolViewModel" />.</returns>
	public async Task<SchoolViewModel?> ViewDetailsAsync(string id)
	{
		var viewModel = await this._dbContext
			.Schools
			.Where(s => s.Id == Guid.Parse(id))
			.Select(s => new SchoolViewModel
			{
				Id = s.Id.ToString(),
				Name = s.Name,
				Address = s.Address,
				Email = s.Email,
				PhoneNumber = s.PhoneNumber,
				IsActive = s.IsActive,
				Classrooms = s.Classrooms
					.Select(c => new ClassroomViewModel
					{
						Id = c.Id.ToString(),
						Name = c.Name,
						TeacherName = c.Teacher.ApplicationUser.GetFullName(),
						Students = c.Students
							.Select(st => new BasicViewModel
							{
								Id = st.Id.ToString(),
								Name = st.ApplicationUser.GetFullName()
							})
							.ToArray()
					})
					.ToArray()
			})
			.AsNoTracking()
			.FirstOrDefaultAsync();

		return viewModel;
	}

	/// <summary>
	///     Deletes a school from the database based on the provided school ID.
	/// </summary>
	/// <param name="id">The ID of the school to delete.</param>
	/// <returns>True if the school is successfully deleted; otherwise, false.</returns>
	public async Task<bool> DeleteAsync(string id)
	{
		var schoolToDelete = await this._dbContext.Schools.FindAsync(Guid.Parse(id));

		if (schoolToDelete != null)
		{
			// Remove the school from the database
			this._dbContext.Schools.Remove(schoolToDelete);
			await this._dbContext.SaveChangesAsync();

			return true; // Return true if the school is successfully deleted
		}

		return false; // Return false if the school to delete is not found
	}

	/// <summary>
	///     Retrieves details of a school based on the provided school ID.
	/// </summary>
	/// <param name="id">The ID of the school to retrieve.</param>
	/// <returns>The details of the school as an instance of <see cref="SchoolViewModel" />.</returns>
	public async Task<SchoolViewModel?> GetByIdAsync(string id)
	{
		var school = await this._dbContext
			.Schools
			.AsNoTracking()
			.FirstOrDefaultAsync(s => s.Id == Guid.Parse(id));

		if (school != null)
		{
			return new SchoolViewModel
			{
				Id = school.Id.ToString(),
				Name = school.Name,
				Address = school.Address,
				Email = school.Email,
				PhoneNumber = school.PhoneNumber,
				IsActive = school.IsActive
			};
		}

		return null; // Return null if the school with the provided ID is not found
	}
}