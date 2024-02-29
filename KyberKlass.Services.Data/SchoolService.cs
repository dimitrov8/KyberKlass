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
    public async Task<bool> AddSchoolAsync(AddSchoolFormModel model)
    {
        var newSchool = new School
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            Address = model.Address,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            IsDeleted = false
        };

        bool schoolExists = await this.SchoolExistAsync(newSchool);

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
    public async Task<IEnumerable<SchoolViewModel>> AllSchoolsAsync()
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
                IsDeleted = false
            })
            .AsNoTracking()
            .ToArrayAsync();

        return schools;
    }

    // Checks if school exists
    public async Task<bool> SchoolExistAsync(School newSchool)
    {
        return await this._dbContext
            .Schools
            .AnyAsync(s => s.Name == newSchool.Name);
    }

    public async Task<AddSchoolFormModel?> GetForEditSchoolAsync(string? id)
    {
        if (id == null)
        {
            return null;
        }

        var viewModel = await this._dbContext
            .Schools
            .Where(s => s.Id.ToString() == id)
            .Select(s => new AddSchoolFormModel
            {
                Name = s.Name,
                Address = s.Address,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                IsDeleted = s.IsDeleted
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return viewModel;
    }

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
        schoolForEdit.IsDeleted = model.IsDeleted;

        await this._dbContext.SaveChangesAsync();

        return true;
    }
}