#region

using KyberKlass.Data;
using KyberKlass.Data.Models;
using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Web.ViewModels.Admin;
using KyberKlass.Web.ViewModels.Admin.Classroom;
using KyberKlass.Web.ViewModels.Admin.User;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

#endregion

namespace KyberKlass.Services.Data;

/// <summary>
///     Service class responsible for managing teachers.
/// </summary>
/// <remarks>
///     Constructor for TeacherService.
/// </remarks>
/// <param name="dbContext">The database context.</param>
/// <param name="userManager">The user manager.</param>
public class TeacherService(KyberKlassDbContext dbContext, UserManager<ApplicationUser> userManager) : ITeacherService
{
    private readonly KyberKlassDbContext _dbContext = dbContext;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    /// <inheritdoc />
    public async Task<IEnumerable<UserViewModel>> AllAsync(string? searchTerm = null)
    {
        const string teacherRoleName = "Teacher";

        Guid teacherRoleId = await _dbContext
            .Roles
            .AsNoTracking()
            .Where(predicate: r => r.Name == teacherRoleName)
            .Select(selector: r => r.Id)
            .FirstOrDefaultAsync();

        if (teacherRoleId != Guid.Empty)
        {
            IQueryable<ApplicationUser> query = from user in _dbContext.Users
                where user.IsActive
                join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                join role in _dbContext.Roles on userRole.RoleId equals role.Id
                where role.Id == teacherRoleId
                select user;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                string term = searchTerm.ToLower();
                query = query.Where(predicate: u =>
                    u.Email != null && (u.FirstName.ToLower().Contains(term) ||
                                        u.LastName.ToLower().Contains(term) ||
                                        u.Email.ToLower().Contains(term)));
            }

            ApplicationUser[] result = await query
                .AsNoTracking()
                .ToArrayAsync();

            return result.Select(selector: t => new UserViewModel
            {
                Id = t.Id.ToString(), Email = t.Email, FullName = t.GetFullName(), Role = teacherRoleName
            });
        }

        return Enumerable.Empty<UserViewModel>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BasicViewModel>> GetUnassignedTeachersAsync()
    {
        IList<ApplicationUser> allTeachers = await _userManager.GetUsersInRoleAsync("Teacher");

        if (!allTeachers.Any())
        {
            return Enumerable.Empty<BasicViewModel>();
        }

        List<Guid> assignedTeacherIds = await _dbContext.Classrooms
            .Select(selector: c => c.TeacherId)
            .ToListAsync();

        List<BasicViewModel> unassignedTeachers = allTeachers
            .Where(predicate: t => !assignedTeacherIds.Contains(t.Id))
            .Select(selector: t => new BasicViewModel { Id = t.Id.ToString(), Name = t.GetFullName() })
            .ToList();

        return unassignedTeachers;
    }

    /// <inheritdoc />
    public async Task<bool> IsTeacherAssignedToClassroomAsync(string userId)
    {
        bool isAssigned = await _dbContext
            .Classrooms
            .AsNoTracking()
            .AnyAsync(predicate: c => c.TeacherId == Guid.Parse(userId));

        return isAssigned;
    }

    public async Task<IEnumerable<ClassroomDetailsViewModel>> GetTeacherClassroomAsync(string teacherId)
    {
        IEnumerable<Classroom> classrooms = await _dbContext
            .Classrooms
            .Where(predicate: c => c.TeacherId == Guid.Parse(teacherId))
            .Include(navigationPropertyPath: c => c.Students)
            .ThenInclude(navigationPropertyPath: s => s.ApplicationUser)
            .Include(navigationPropertyPath: c => c.Teacher.ApplicationUser)
            .AsNoTracking()
            .ToArrayAsync();

        return classrooms.Select(selector: c => new ClassroomDetailsViewModel
        {
            Id = c.Id.ToString(),
            Name = c.Name,
            TeacherName = c.Teacher.ApplicationUser.GetFullName(),
            IsActive = c.IsActive,
            Students = c.Students
                .Select(selector: s => new BasicViewModel
                {
                    Id = s.Id.ToString(), Name = s.ApplicationUser.GetFullName()
                })
                .ToArray()
        });
    }
}