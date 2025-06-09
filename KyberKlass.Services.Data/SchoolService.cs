using KyberKlass.Data;
using KyberKlass.Data.Models;
using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Web.ViewModels.Admin;
using KyberKlass.Web.ViewModels.Admin.Classroom;
using KyberKlass.Web.ViewModels.Admin.School;
using Microsoft.EntityFrameworkCore;

namespace KyberKlass.Services.Data;
/// <summary>
///     Service class responsible for managing schools.
/// </summary>
public class SchoolService : ISchoolService
{
    private readonly KyberKlassDbContext _dbContext;

    /// <summary>
    ///     Constructor for ClassroomService.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public SchoolService(KyberKlassDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SchoolViewModel>> AllAsync(string? searchTerm = null)
    {
        IQueryable<School> query = _dbContext
            .Schools
            .AsNoTracking()
            .Where(s => s.IsActive == true); // Filter to include only active schools

        if (!string.IsNullOrEmpty(searchTerm))
        {
            string term = searchTerm.ToLower();

            query = query.Where(s =>
            s.Name.ToLower().Contains(term) ||
            s.Address.ToLower().Contains(term) ||
            s.Email.ToLower().Contains(term) ||
            s.PhoneNumber.Contains(term));
        }

        IEnumerable<SchoolViewModel> viewModel = await query
            .Select(s => new SchoolViewModel
            {
                Id = s.Id.ToString(),
                Name = s.Name,
                Address = s.Address,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber
            })
            .ToArrayAsync();

        return viewModel;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BasicViewModel>> BasicAllAsync()
    {
        return await _dbContext
            .Schools
            .Select(s => new BasicViewModel
            {
                Id = s.Id.ToString(),
                Name = s.Name
            })
            .AsNoTracking()
            .ToArrayAsync();
    }

    /// <inheritdoc />
    public async Task<bool> AddAsync(AddSchoolFormModel model)
    {
        School newSchool = new()
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            Address = model.Address,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            IsActive = true
        }; // Create a new School object with data from the provided model

        // Check if a school with the same name already exists
        bool schoolExists = await _dbContext.Schools.AnyAsync(s => s.Name == model.Name);

        if (schoolExists)
        {
            return false;
        }

        // Add the new school to the database and save changes
        await _dbContext.Schools.AddAsync(newSchool);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc />
    public async Task<SchoolDetailsViewModel?> ViewDetailsAsync(string id)
    {
        SchoolDetailsViewModel? viewModel = await _dbContext
            .Schools
            .Where(s => s.Id == Guid.Parse(id))
            .Select(s => new SchoolDetailsViewModel
            {
                Id = s.Id.ToString(),
                Name = s.Name,
                Address = s.Address,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                IsActive = s.IsActive,
                Classrooms = s.Classrooms
                    .Select(c => new ClassroomDetailsViewModel
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

    /// <inheritdoc />
    public async Task<AddSchoolFormModel?> GetForEditAsync(string id)
    {
        AddSchoolFormModel? viewModel = await _dbContext
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

    /// <inheritdoc />
    public async Task<bool> EditAsync(string id, AddSchoolFormModel model)
    {
        School? schoolForEdit = await _dbContext.Schools.FindAsync(Guid.Parse(id));

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

        await _dbContext.SaveChangesAsync(); // Save changes to the database

        return true; // Return true if the school is successfully edited
    }

    /// <inheritdoc />
    public async Task<SchoolDetailsViewModel?> GetForDeleteAsync(string id)
    {
        SchoolDetailsViewModel? viewModel = await ViewDetailsAsync(id);

        return viewModel;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(string id)
    {
        School? schoolToDelete = await _dbContext.Schools.FindAsync(Guid.Parse(id));

        if (schoolToDelete != null && schoolToDelete.Students.Any() == false && schoolToDelete.Classrooms.Any() == false)
        {
            // Remove the school from the database
            _dbContext.Schools.Remove(schoolToDelete);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public async Task<bool> HasStudentsAssignedAsync(string id)
    {
        School? school = await _dbContext
            .Schools
            .FirstOrDefaultAsync(s => s.Id == Guid.Parse(id));

        return school != null && school.Students.Any();
    }

    /// <inheritdoc />
    public async Task<SchoolDetailsViewModel?> GetByIdAsync(string id)
    {
        School? school = await _dbContext
            .Schools
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == Guid.Parse(id));

        return school != null
            ? new SchoolDetailsViewModel
            {
                Id = school.Id.ToString(),
                Name = school.Name,
                Address = school.Address,
                Email = school.Email,
                PhoneNumber = school.PhoneNumber,
                IsActive = school.IsActive
            }
            : null;
    }

    /// <inheritdoc />
    public async Task<bool> ClassroomExistsInSchoolAsync(string schoolId, string classroomId)
    {
        return await _dbContext
            .Schools
            .AsNoTracking()
            .AnyAsync(s => s.Id == Guid.Parse(schoolId) && s.Classrooms.Any(c => c.Id == Guid.Parse(classroomId)));
    }

    public Task<IEnumerable<SchoolDetailsViewModel>> AllAsync()
    {
        throw new NotImplementedException();
    }
}