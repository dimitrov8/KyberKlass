#region

using KyberKlass.Data;
using KyberKlass.Data.Models;
using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Web.ViewModels.Admin;
using KyberKlass.Web.ViewModels.Admin.Classroom;
using KyberKlass.Web.ViewModels.Admin.School;

using Microsoft.EntityFrameworkCore;

#endregion

namespace KyberKlass.Services.Data;

/// <summary>
///     Service class responsible for managing schools.
/// </summary>
/// <remarks>
///     Constructor for ClassroomService.
/// </remarks>
/// <param name="dbContext">The database context.</param>
public class SchoolService(KyberKlassDbContext dbContext) : ISchoolService
{
    public async Task<IEnumerable<SchoolDetailsViewModel>> AllAsync(string? searchTerm)
    {
        IQueryable<School> query = dbContext
            .Schools
            .AsNoTracking()
            .Where(predicate: s => s.IsActive == true); // Filter to include only active schools

        if (!string.IsNullOrEmpty(searchTerm))
        {
            string term = searchTerm.ToLower();

            query = query.Where(predicate: s =>
                s.Name.ToLower().Contains(term) ||
                s.Address.ToLower().Contains(term) ||
                s.Email.ToLower().Contains(term) ||
                s.PhoneNumber.Contains(term));
        }

        IEnumerable<SchoolDetailsViewModel> viewModel = await query
            .Select(selector: s => new SchoolDetailsViewModel
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
                                Id = st.Id.ToString(), Name = st.ApplicationUser.GetFullName()
                            })
                            .ToArray()
                    })
                    .ToArray()
            })
            .ToArrayAsync();

        return viewModel;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BasicViewModel>> BasicAllAsync()
    {
        return await dbContext
            .Schools
            .Select(selector: s => new BasicViewModel { Id = s.Id.ToString(), Name = s.Name })
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
        bool schoolExists = await dbContext.Schools.AnyAsync(predicate: s => s.Name == model.Name);

        if (schoolExists)
        {
            return false;
        }

        // Add the new school to the database and save changes
        await dbContext.Schools.AddAsync(newSchool);
        await dbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc />
    public async Task<SchoolDetailsViewModel?> ViewDetailsAsync(string id)
    {
        SchoolDetailsViewModel? viewModel = await dbContext
            .Schools
            .Where(predicate: s => s.Id == Guid.Parse(id))
            .Select(selector: s => new SchoolDetailsViewModel
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
                                Id = st.Id.ToString(), Name = st.ApplicationUser.GetFullName()
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
        AddSchoolFormModel? viewModel = await dbContext
            .Schools
            .Where(predicate: s => s.Id == Guid.Parse(id))
            .Select(selector: s => new AddSchoolFormModel
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
        School? schoolForEdit = await dbContext.Schools.FindAsync(Guid.Parse(id));

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

        await dbContext.SaveChangesAsync(); // Save changes to the database

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
        School? schoolToDelete = await dbContext.Schools.FindAsync(Guid.Parse(id));

        if (schoolToDelete != null && !schoolToDelete.Students.Any() && !schoolToDelete.Classrooms.Any())
        {
            // Remove the school from the database
            dbContext.Schools.Remove(schoolToDelete);
            await dbContext.SaveChangesAsync();

            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public async Task<bool> HasStudentsAssignedAsync(string id)
    {
        School? school = await dbContext
            .Schools.Include(navigationPropertyPath: school => school.Students)
            .FirstOrDefaultAsync(predicate: s => s.Id == Guid.Parse(id));

        return school != null && school.Students.Any();
    }

    /// <inheritdoc />
    public async Task<SchoolDetailsViewModel?> GetByIdAsync(string id)
    {
        School? school = await dbContext
            .Schools
            .AsNoTracking()
            .FirstOrDefaultAsync(predicate: s => s.Id == Guid.Parse(id));

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
        return await dbContext
            .Schools
            .AsNoTracking()
            .AnyAsync(predicate: s
                => s.Id == Guid.Parse(schoolId) && s.Classrooms.Any(c => c.Id == Guid.Parse(classroomId)));
    }
}