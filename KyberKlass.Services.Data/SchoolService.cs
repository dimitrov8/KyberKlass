namespace KyberKlass.Services.Data;

using Interfaces;
using KyberKlass.Data;
using KyberKlass.Data.Models;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.Admin.School;

public class SchoolService : ISchoolService
{
	private readonly KyberKlassDbContext _dbContext;

	public SchoolService(KyberKlassDbContext dbContext)
	{
		this._dbContext = dbContext;
	}

    /// <summary>
    /// Adds a new school to the database based on the provided AddSchoolFormModel.
    /// </summary>
    /// <param name="model">The AddSchoolFormModel containing the data for the new school.</param>
    /// <returns>True if the school was successfully added, false if the school already exists.</returns>
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
		}; // Create a new School object with data from the AddSchoolFormModel

        bool schoolExists = await this.ExistAsync(newSchool); // Check if the school already exists

        if (schoolExists)
		{
			return false; // School already exists, return false
        }

        // Add the new school to the Schools DbSet and save changes
        await this._dbContext.Schools.AddAsync(newSchool); 
		await this._dbContext.SaveChangesAsync();
		 
		return true; // School added successfully, return true
    }

    /// <summary>
    /// Retrieves all schools from the database and maps them to SchoolViewModels.
    /// </summary>
    /// <returns>An IEnumerable of SchoolViewModels representing all schools.</returns>
    public async Task<IEnumerable<SchoolViewModel>> AllAsync()
	{
		IEnumerable<SchoolViewModel> schools = await this._dbContext
			.Schools
			.Select(s => new SchoolViewModel
			{
				Id = s.Id.ToString(),
				Name = s.Name,
				Address = s.Address,
				Email = s.Email,
				PhoneNumber = s.PhoneNumber,
				IsActive = true
			})
			.AsNoTracking()
			.ToArrayAsync(); // Retrieve all schools from the database and map them to SchoolViewModels

        return schools; // Return the mapped SchoolViewModels
    }

    /// <summary>
    /// Checks if a school with the same name as the specified new school already exists.
    /// </summary>
    /// <param name="newSchool">The new school to check for existence.</param>
    /// <returns>True if a school with the same name already exists, false otherwise.</returns>
    public async Task<bool> ExistAsync(School newSchool)
	{
		return await this._dbContext
			.Schools
			.AsNoTracking()
			.AnyAsync(s => s.Name == newSchool.Name); // Check if any school with the same name as the new school exists in the database
    }

    /// <summary>
    /// Retrieves the school with the specified ID for editing purposes.
    /// </summary>
    /// <param name="id">The ID of the school to retrieve.</param>
    /// <returns>An AddSchoolFormModel representing the school to edit, or null if not found.</returns>
    public async Task<AddSchoolFormModel?> GetForEditAsync(string? id)
	{
		var viewModel = await this._dbContext
			.Schools
			.Where(s => s.Id.ToString() == id)
			.Select(s => new AddSchoolFormModel
			{
				Name = s.Name,
				Address = s.Address,
				Email = s.Email,
				PhoneNumber = s.PhoneNumber,
				IsActive = s.IsActive
			})
			.AsNoTracking()
			.FirstOrDefaultAsync(); // Retrieve the school with the specified ID from the database

        return viewModel; // Return the AddSchoolFormModel for editing, or null if not found
    }

    /// <summary>
    /// Retrieves the details of a school for deletion based on the specified ID.
    /// </summary>
    /// <param name="id">The ID of the school to retrieve.</param>
    /// <returns>A <see cref="SchoolViewModel"/> representing the school to be deleted, or null if the school does not exist.</returns>
    public async Task<SchoolViewModel?> GetForDeleteAsync(string? id)
	{
		var viewModel = await this._dbContext
			.Schools
			.Where(s => s.Id.ToString() == id)
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
			.FirstOrDefaultAsync(); // Retrieve the school details based on the provided I

        return viewModel; // Return the SchoolViewModel for deleting, or null if not found
    }

    /// <summary>
    /// Edits an existing school based on the provided ID and updated information.
    /// </summary>
    /// <param name="id">The ID of the school to edit.</param>
    /// <param name="model">A <see cref="SchoolViewModel"/> containing the updated school information.</param>
    /// <returns>True if the school was successfully edited, otherwise false.</returns>
    public async Task<bool> EditSchoolAsync(string id, SchoolViewModel model)
	{
		var schoolForEdit = await this._dbContext
			.Schools
			.FirstOrDefaultAsync(s => s.Id.ToString() == id); // Retrieve the school to edit based on the provided ID

        if (schoolForEdit == null)
		{
			return false; // Return false if the school to edit does not exist
        }

        // Update the school information with the provided data
        schoolForEdit.Name = model.Name;
		schoolForEdit.Address = model.Address;
		schoolForEdit.Email = model.Email;
		schoolForEdit.PhoneNumber = model.PhoneNumber;
		schoolForEdit.IsActive = model.IsActive;

		await this._dbContext.SaveChangesAsync(); // Save changes to update the school in the database

        return true; // Return true indicating successful editing of the school
    }

    /// <summary>
    /// Retrieves the details of a school based on the provided ID.
    /// </summary>
    /// <param name="id">The ID of the school to retrieve details for.</param>
    /// <returns>A <see cref="SchoolViewModel"/> containing the details of the school, or null if the school is not found.</returns>
    public async Task<SchoolViewModel?> ViewDetailsAsync(string id)
	{
		var viewModel = await this._dbContext
			.Schools
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
			.FirstOrDefaultAsync(s => s.Id == id);  // Retrieve the details of the school with the provided ID

        return viewModel; // Return the school details or null if the school is not found
    }

    /// <summary>
    /// Deletes a school from the database based on the provided ID.
    /// </summary>
    /// <param name="id">The ID of the school to delete.</param>
    /// <returns>True if the school is successfully deleted, otherwise false.</returns>
    public async Task<bool> DeleteAsync(string? id)
    {
        var schoolToDelete = await this._dbContext
            .Schools
            .FirstOrDefaultAsync(s => s.Id.ToString() == id); // Retrieve the school to delete based on the provided ID

        if (schoolToDelete != null)
		{
            // Remove the school from the database
            this._dbContext.Schools.Remove(schoolToDelete);
            await this._dbContext.SaveChangesAsync();

			return true; // Return true if the school is successfully deleted
        }

		return false; // Return false if the school with the provided ID is not found
    }

	public async Task<bool> IsNotNullOrEmptyInputAsync(string? id, SchoolViewModel? model)
	{
		if (string.IsNullOrEmpty(id))
		{
			return false;
		}

		if (model != null && string.IsNullOrEmpty(model.Id))
		{
			return false;
		}

		return true;
	}

    /// <summary>
    /// Retrieves a school from the database based on the provided ID.
    /// </summary>
    /// <param name="id">The ID of the school to retrieve.</param>
    /// <returns>A SchoolViewModel object representing the retrieved school, or null if the school is not found.</returns>
    public async Task<SchoolViewModel?> GetByIdAsync(string id)
    {
        var school = await this._dbContext
            .Schools
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id.ToString() == id); // Retrieve the school from the database based on the provided ID

        if (school != null)
		{
            // Map the school entity to a SchoolViewModel object
            return new SchoolViewModel
			{
				Id = school.Id.ToString(),
				Name = school.Name,
				Address = school.Address,
				Email = school.Email,
				PhoneNumber = school.PhoneNumber,
				IsActive = school.IsActive,
			};
		}

		return null; // Return null if the school with the provided ID is not found
    }
}