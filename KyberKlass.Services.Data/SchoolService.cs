namespace KyberKlass.Services.Data;

using Interfaces;
using KyberKlass.Data;
using KyberKlass.Data.Models;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.Admin;
using Web.ViewModels.Admin.Classroom;
using Web.ViewModels.Admin.School;

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
        this._dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SchoolDetailsViewModel>> AllAsync()
    {
        School[] schools = await this._dbContext
            .Schools
            .Include(s => s.Classrooms)
            .ThenInclude(c => c.Teacher.ApplicationUser)
            .Include(s => s.Classrooms)
            .ThenInclude(c => c.Students)
            .AsNoTracking()
            .ToArrayAsync();

        IEnumerable<SchoolDetailsViewModel> viewModel = schools
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
            });

        return viewModel;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BasicViewModel>> BasicAllAsync()
    {
        return await this._dbContext
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
        await this._dbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc />
    public async Task<SchoolDetailsViewModel?> ViewDetailsAsync(string id)
    {
        var viewModel = await this._dbContext
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

    /// <inheritdoc />
    public async Task<bool> EditAsync(string id, AddSchoolFormModel model)
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

    /// <inheritdoc />
    public async Task<SchoolDetailsViewModel?> GetForDeleteAsync(string id)
    {
        var viewModel = await this.ViewDetailsAsync(id);

        return viewModel;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(string id)
    {
        var schoolToDelete = await this._dbContext.Schools.FindAsync(Guid.Parse(id));

        if (schoolToDelete != null && schoolToDelete.Students.Any() == false && schoolToDelete.Classrooms.Any() == false)
        {
            // Remove the school from the database
            this._dbContext.Schools.Remove(schoolToDelete);
            await this._dbContext.SaveChangesAsync();

            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public async Task<bool> HasStudentsAssignedAsync(string id)
    {
        var school = await this._dbContext
            .Schools
            .FirstOrDefaultAsync(s => s.Id == Guid.Parse(id));

        return school != null && school.Students.Any();
    }

    /// <inheritdoc />
    public async Task<SchoolDetailsViewModel?> GetByIdAsync(string id)
    {
        var school = await this._dbContext
            .Schools
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == Guid.Parse(id));

        if (school != null)
        {
            return new SchoolDetailsViewModel
            {
                Id = school.Id.ToString(),
                Name = school.Name,
                Address = school.Address,
                Email = school.Email,
                PhoneNumber = school.PhoneNumber,
                IsActive = school.IsActive
            };
        }

        return null;
    }

    /// <inheritdoc />
    public async Task<bool> ClassroomExistsInSchoolAsync(string schoolId, string classroomId)
    {
        return this._dbContext
            .Schools
            .AsNoTracking()
            .Any(s => s.Id == Guid.Parse(schoolId) && s.Classrooms.Any(c => c.Id == Guid.Parse(classroomId)));
    }
}