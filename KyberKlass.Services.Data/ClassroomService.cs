namespace KyberKlass.Services.Data;

using Interfaces;
using KyberKlass.Data;
using KyberKlass.Data.Models;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.Admin.Classroom;

/// <summary>
///     Service class responsible for managing classrooms.
/// </summary>
public class ClassroomService : IClassroomService
{
    private readonly KyberKlassDbContext _dbContext;
    private readonly ISchoolService _schoolService;

    /// <summary>
    ///     Constructor for ClassroomService.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="schoolService">The service for managing schools.</param>
    public ClassroomService(KyberKlassDbContext dbContext, ISchoolService schoolService)
    {
        this._dbContext = dbContext;
        this._schoolService = schoolService;
    }

    /// <summary>
    ///     Retrieves a view model containing classrooms for a specified school.
    /// </summary>
    /// <param name="schoolId">The ID of the school.</param>
    /// <returns>A view model containing classrooms for the school.</returns>
    public async Task<ManageClassroomsViewModel> GetManageClassroomsAsync(string schoolId)
    {
        var school = await this._schoolService.GetByIdAsync(schoolId); // Retrieve the school
        IEnumerable<ClassroomViewModel> classrooms = await this.GetClassroomsAsync(schoolId); // Retrieve classrooms for the school

        return new ManageClassroomsViewModel
        {
            SchoolId = schoolId,
            SchoolName = school!.Name,
            Classrooms = classrooms
                .Select(c => new ClassroomViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
        }; // Create and return a view model containing classrooms
    }


    public async Task<IEnumerable<ClassroomViewModel>> GetClassroomsAsync(string schoolId)
    {
        Classroom[] classrooms = await this._dbContext
            .Classrooms
            .Where(c => c.SchoolId == Guid.Parse(schoolId))
            .Include(c => c.Students)
            .Include(u => u.Teacher.ApplicationUser)
            .AsNoTracking()
            .ToArrayAsync();

        IEnumerable<ClassroomViewModel> classroomViewModels = classrooms.Select(c => new ClassroomViewModel
        {
            Id = c.Id.ToString(),
            Name = c.Name,
            TeacherName = c.Teacher.ApplicationUser.GetFullName(),
            StudentsCount = c.Students.Count()
        });

        return classroomViewModels;
    }

    public async Task<IEnumerable<ClassroomBasicViewModel>> GetAllSchoolClassroomsAsync()
    {
        IEnumerable<ClassroomBasicViewModel> allClassrooms = await this._dbContext
            .Classrooms
            .Select(c => new ClassroomBasicViewModel
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
        var newSchool = await this._schoolService.GetByIdAsync(model.SchoolId); // Retrieve the school by ID

        if (newSchool == null)
        {
            return false; // If the school doesn't exist, return false
        }

        bool classRoomExists = await this.ClassroomExistsInSchool(model.Name, model.SchoolId); // Check if the classroom already exists in the school

        if (classRoomExists)
        {
            return false; // If the classroom already exists, return false
        }

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
    public async Task<bool> ClassroomExistsInSchool(string classroomName, string schoolId)
    {
        return await this._dbContext
            .Classrooms
            .AsNoTracking()
            .AnyAsync(c => c.Name == classroomName && c.SchoolId == Guid.Parse(schoolId));
        // Check if any classroom with the given name and school ID exists in the database
    }
}