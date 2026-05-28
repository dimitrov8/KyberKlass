using KyberKlass.Data;
using KyberKlass.Data.Models;
using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Web.ViewModels.Admin;
using KyberKlass.Web.ViewModels.Admin.Classroom;
using Microsoft.EntityFrameworkCore;

namespace KyberKlass.Services.Data;
/// <summary>
///     Service class responsible for managing classrooms.
/// </summary>
/// <remarks>
///     Constructor for ClassroomService.
/// </remarks>
/// <param name="dbContext">The database context.</param>
public class ClassroomService(KyberKlassDbContext dbContext) : IClassroomService
{
    private readonly KyberKlassDbContext _dbContext = dbContext;
    /// <inheritdoc />
    public async Task<bool> AddAsync(AddClassroomViewModel model)
    {
        School? newSchool = await _dbContext.Schools.FindAsync(Guid.Parse(model.SchoolId));

        if (newSchool == null)
        {
            return false; // If the school doesn't exist, return false
        }

        Guid schoolIdAsGuid = Guid.Parse(model.SchoolId);
        Guid teacherIdAsGuid = Guid.Parse(model.TeacherId);

        Classroom newClassroom = new()
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            TeacherId = teacherIdAsGuid,
            SchoolId = schoolIdAsGuid
        };

        await _dbContext.Classrooms.AddAsync(newClassroom);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> ClassroomExistsInSchoolAsync(string classroomName, string schoolId)
    {
        return await _dbContext
            .Classrooms
            .AsNoTracking()
            .AnyAsync(c => c.Name == classroomName && c.SchoolId == Guid.Parse(schoolId));
    }

    /// <inheritdoc />
    public async Task<ClassroomDetailsViewModel?> GetClassroomAsync(string id)
    {
        return await _dbContext
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
        IEnumerable<Classroom> classrooms = await _dbContext
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
        IEnumerable<BasicViewModel> allClassrooms = await _dbContext
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
        AddClassroomViewModel? viewModel = await _dbContext
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
        Classroom? classroomToEdit = await _dbContext.Classrooms.FindAsync(Guid.Parse(id));

        if (classroomToEdit == null)
        {
            return false;
        }

        classroomToEdit.IsActive = model.IsActive;

        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc />
    public async Task<ClassroomDetailsViewModel?> GetForDeleteAsync(string id)
    {
        ClassroomDetailsViewModel? viewModel = await GetClassroomAsync(id);

        return viewModel;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(string id)
    {
        Classroom? classroomToDelete = await _dbContext
            .Classrooms.Include(classroom => classroom.Students)
            .FirstOrDefaultAsync(c => c.Id == Guid.Parse(id));

        // If the classroom exists and there are no students in this classrooms
        if (classroomToDelete != null && classroomToDelete.Students.Any() == false)
        {
            _dbContext.Classrooms.Remove(classroomToDelete);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public async Task<bool> HasStudentsAssignedAsync(string id)
    {
        bool hasStudents = await _dbContext
            .Classrooms
            .Where(c => c.Id == Guid.Parse(id))
            .AnyAsync(c => c.Students.Any());

        return hasStudents;
    }

    public async Task<IEnumerable<ClassroomDetailsViewModel>> GetTeacherClassroomsAsync(string? teacherId)
    {
        IEnumerable<Classroom> classrooms = await _dbContext
            .Classrooms
            .Where(c => c.TeacherId == Guid.Parse(teacherId!))
            .Include(c => c.Students)
            .ThenInclude(s => s.ApplicationUser)
            .Include(c => c.Teacher.ApplicationUser)
            .AsNoTracking()
            .ToArrayAsync();

        return classrooms.Select(c => new ClassroomDetailsViewModel
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
    }

    public async Task<IEnumerable<BasicViewModel>> GetClassroomStudentsAsync(string classroomId)
    {
        IEnumerable<Student> students = await _dbContext
            .Students
            .Where(s => s.ClassroomId == Guid.Parse(classroomId))
            .Include(s => s.ApplicationUser)
            .AsNoTracking()
            .ToArrayAsync();

        return students.Select(s => new BasicViewModel
        {
            Id = s.Id.ToString(),
            Name = s.ApplicationUser.GetFullName()
        });
    }
}