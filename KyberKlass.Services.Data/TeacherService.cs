using KyberKlass.Data;
using KyberKlass.Data.Models;
using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Web.ViewModels.Admin;
using KyberKlass.Web.ViewModels.Admin.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KyberKlass.Services.Data;
/// <summary>
///     Service class responsible for managing teachers.
/// </summary>
public class TeacherService : ITeacherService
{
    private readonly KyberKlassDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    ///     Constructor for TeacherService.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="userManager">The user manager.</param>
    public TeacherService(KyberKlassDbContext dbContext, UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserViewModel>> AllAsync(string? searchTerm = null)
    {
        string teacherRoleName = "Teacher";

        Guid teacherRoleId = await _dbContext
            .Roles
            .AsNoTracking()
            .Where(r => r.Name == teacherRoleName)
            .Select(r => r.Id)
            .FirstOrDefaultAsync();

        if (teacherRoleId != Guid.Empty)
        {
            var query = from user in _dbContext.Users
                        where user.IsActive
                        join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                        join role in _dbContext.Roles on userRole.RoleId equals role.Id
                        where role.Id == teacherRoleId
                        select user;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                string term = searchTerm.ToLower();
                query = query.Where(u =>
                    u.FirstName.ToLower().Contains(term) ||
                    u.LastName.ToLower().Contains(term) ||
                    u.Email.ToLower().Contains(term));
            }

            var result = await query
                .AsNoTracking()
                .ToArrayAsync();

            return result.Select(t => new UserViewModel
            {
                Id = t.Id.ToString(),
                Email = t.Email,
                FullName = t.GetFullName(),
                Role = teacherRoleName,
            });
        }

        return Enumerable.Empty<UserViewModel>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BasicViewModel>> GetUnassignedTeachersAsync()
    {
        IList<ApplicationUser> allTeachers = await _userManager.GetUsersInRoleAsync("Teacher");

        if (allTeachers.Any() == false)
        {
            return Enumerable.Empty<BasicViewModel>();
        }

        List<Guid> assignedTeacherIds = await _dbContext.Classrooms
            .Select(c => c.TeacherId)
            .ToListAsync();

        List<BasicViewModel> unassignedTeachers = allTeachers
            .Where(t => assignedTeacherIds.Contains(t.Id) == false)
            .Select(t => new BasicViewModel
            {
                Id = t.Id.ToString(),
                Name = t.GetFullName()
            })
            .ToList();

        return unassignedTeachers;
    }

    /// <inheritdoc />
    public async Task<bool> IsTeacherAssignedToClassroomAsync(string userId)
    {
        bool isAssigned = await _dbContext
            .Classrooms
            .AsNoTracking()
            .AnyAsync(c => c.TeacherId == Guid.Parse(userId));

        return isAssigned;
    }
}