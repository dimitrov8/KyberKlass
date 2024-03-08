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

	// Adding school
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
		};

		bool schoolExists = await this.ExistAsync(newSchool);

		if (schoolExists)
		{
			return false;
		}

		await this._dbContext
			.Schools
			.AddAsync(newSchool);

		await this._dbContext
			.SaveChangesAsync();

		return true;
	}

	// Visualizing schools
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
			.ToArrayAsync();

		return schools;
	}

	// Checks if school exists
	public async Task<bool> ExistAsync(School newSchool)
	{
		return await this._dbContext
			.Schools
			.AnyAsync(s => s.Name == newSchool.Name);
	}

	// Gets school for editing
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
			.FirstOrDefaultAsync();

		return viewModel;
	}

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
			.FirstOrDefaultAsync();

		return viewModel;
	}

// Editing school
	public async Task<bool> EditSchoolAsync(string id, SchoolViewModel model)
	{
		var schoolForEdit = await this._dbContext
			.Schools
			.FirstOrDefaultAsync(s => s.Id.ToString() == id);

		if (schoolForEdit == null)
		{
			return false;
		}

		schoolForEdit.Name = model.Name;
		schoolForEdit.Address = model.Address;
		schoolForEdit.Email = model.Email;
		schoolForEdit.PhoneNumber = model.PhoneNumber;
		schoolForEdit.IsActive = model.IsActive;

		await this._dbContext.SaveChangesAsync();

		return true;
	}

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
			.FirstOrDefaultAsync(s => s.Id == id);

		return viewModel;
	}

	public async Task<bool> DeleteAsync(string? id)
	{
		var schoolToDelete = await this._dbContext
			.Schools
			.FirstOrDefaultAsync(s => s.Id.ToString() == id);

		if (schoolToDelete != null)
		{
			this._dbContext.Schools.Remove(schoolToDelete);
			await this._dbContext.SaveChangesAsync();

			return true;
		}

		return false;
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
}