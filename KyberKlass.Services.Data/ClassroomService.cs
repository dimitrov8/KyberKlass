﻿namespace KyberKlass.Services.Data;

using Interfaces;
using KyberKlass.Data;
using KyberKlass.Data.Models;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.Admin;
using Web.ViewModels.Admin.Classroom;

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

    /// <inheritdoc />
    public async Task<bool> AddAsync(AddClassroomViewModel model)
    {
        var newSchool = await this._dbContext.Schools.FindAsync(Guid.Parse(model.SchoolId));

        if (newSchool == null)
        {
            return false; // If the school doesn't exist, return false
        }

        var schoolIdAsGuid = Guid.Parse(model.SchoolId);
        var teacherIdAsGuid = Guid.Parse(model.TeacherId);

        var newClassroom = new Classroom
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            TeacherId = teacherIdAsGuid,
            SchoolId = schoolIdAsGuid
        };

        await this._dbContext.Classrooms.AddAsync(newClassroom);
        await this._dbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> ClassroomExistsInSchoolAsync(string classroomName, string schoolId)
    {
        return await this._dbContext
            .Classrooms
            .AsNoTracking()
            .AnyAsync(c => c.Name == classroomName && c.SchoolId == Guid.Parse(schoolId));
    }

    /// <inheritdoc />
    public async Task<ClassroomDetailsViewModel?> GetClassroomAsync(string id)
    {
        return await this._dbContext
            .Classrooms
            .Where(c => c.Id == Guid.Parse(id))
            .Select(c => new ClassroomDetailsViewModel
            {
                Id = c.Id.ToString(),
                SchoolId = c.SchoolId.ToString(),
                Name = c.Name,
                TeacherName = c.Teacher.ApplicationUser.GetFullName(),
                IsActive = c.IsActive,
                Students = c.Students
                    .Select(s => new BasicViewModel
                    {
                        Id = s.Id.ToString(),
                        Name = s.ApplicationUser.GetFullName()
                    })
                    .ToArray()
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ClassroomDetailsViewModel>> AllAsync(string schoolId)
    {
        IEnumerable<Classroom> classrooms = await this._dbContext
            .Classrooms
            .Where(c => c.SchoolId == Guid.Parse(schoolId))
            .Include(c => c.Students)
            .ThenInclude(s => s.ApplicationUser)
            .Include(u => u.Teacher.ApplicationUser)
            .AsNoTracking()
            .ToArrayAsync();

        IEnumerable<ClassroomDetailsViewModel> classroomViewModels = classrooms.Select(c => new ClassroomDetailsViewModel
        {
            Id = c.Id.ToString(),
            Name = c.Name,
            TeacherName = c.Teacher.ApplicationUser.GetFullName(),
            IsActive = c.IsActive,
            Students = c.Students
                .Select(s => new BasicViewModel
                {
                    Id = s.Id.ToString(),
                    Name = s.ApplicationUser.GetFullName()
                })
                .ToArray()
        });

        return classroomViewModels;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BasicViewModel>> GetAllClassroomsBySchoolIdAsJsonAsync(string schoolId)
    {
        IEnumerable<BasicViewModel> allClassrooms = await this._dbContext
            .Classrooms
            .Where(c => c.SchoolId == Guid.Parse(schoolId))
            .Select(c => new BasicViewModel
            {
                Id = c.Id.ToString(),
                Name = c.Name
            })
            .AsNoTracking()
            .ToArrayAsync();

        return allClassrooms;
    }

    /// <inheritdoc />
    public async Task<AddClassroomViewModel?> GetForEditAsync(string id)
    {
        var viewModel = await this._dbContext
            .Classrooms
            .Where(c => c.Id == Guid.Parse(id))
            .Select(c => new AddClassroomViewModel
            {
                Id = c.Id,
                Name = c.Name,
                TeacherId = c.TeacherId.ToString(),
                IsActive = c.IsActive,
                SchoolId = c.SchoolId.ToString()
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return viewModel;
    }

    /// <inheritdoc />
    public async Task<bool> EditAsync(string id, AddClassroomViewModel model)
    {
        var classroomToEdit = await this._dbContext.Classrooms.FindAsync(Guid.Parse(id));

        if (classroomToEdit == null)
        {
            return false;
        }

        classroomToEdit.IsActive = model.IsActive;

        await this._dbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc />
    public async Task<ClassroomDetailsViewModel?> GetForDeleteAsync(string id)
    {
        var viewModel = await this.GetClassroomAsync(id);

        return viewModel;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(string id)
    {
        var classroomToDelete = await this._dbContext
            .Classrooms
            .FirstOrDefaultAsync(c => c.Id == Guid.Parse(id));

        // If the classroom exists and there are no students in this classrooms
        if (classroomToDelete != null && classroomToDelete.Students.Any() == false)
        {
            this._dbContext.Classrooms.Remove(classroomToDelete);
            await this._dbContext.SaveChangesAsync();

            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public async Task<bool> HasStudentsAssignedAsync(string id)
    {
        bool hasStudents = await this._dbContext
            .Classrooms
            .Where(c => c.Id == Guid.Parse(id))
            .AnyAsync(c => c.Students.Any());

        return hasStudents;
    }
}