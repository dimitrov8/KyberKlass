﻿namespace KyberKlass.Services.Data;

using Interfaces;
using KyberKlass.Data;
using KyberKlass.Data.Models;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.Admin;
using Web.ViewModels.Admin.Classroom;

/// <summary>
/// Service class responsible for managing classrooms.
/// </summary>
public class ClassroomService : IClassroomService
{
    private readonly KyberKlassDbContext _dbContext;

    /// <summary>
    /// Constructor for ClassroomService.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public ClassroomService(KyberKlassDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ClassroomDetailsViewModel>> GetAllClassroomsAsync(string schoolId)
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
            Students = c.Students // todo classrooms
                .Select(s => new BasicViewModel
                {
                    Id = s.Id.ToString(),
                    Name = s.ApplicationUser.GetFullName(),
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
    public async Task<bool> AddAsync(AddClassroomViewModel model)
    {
        var newSchool = await this._dbContext.Schools.FindAsync(Guid.Parse(model.SchoolId));

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

        // Add the new classroom to the database
        await this._dbContext.Classrooms.AddAsync(newClassroom);
        await this._dbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> ClassroomExistsInSchoolAsync(string classroomName, string schoolId)
    {
        return await this._dbContext
            .Classrooms
            .AnyAsync(c => c.Name == classroomName && c.SchoolId == Guid.Parse(schoolId));
    }


    // TODO ADD SUMMARY AND MAYBE MAKE ALSO A GO FOR DELETE
    public async Task<bool> DeleteAsync(string classroomId)
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

    /// <inheritdoc />
    public async Task<ClassroomDetailsViewModel?> GetClassroomAsync(string schoolId, string classroomId)
    {
        return await this._dbContext
            .Classrooms
            .Where(c => c.SchoolId == Guid.Parse(schoolId) && c.Id == Guid.Parse(classroomId))
            .Select(c => new ClassroomDetailsViewModel
            {
                Id = c.Id.ToString(),
                SchoolId = schoolId,
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
            .FirstOrDefaultAsync();
    }
}