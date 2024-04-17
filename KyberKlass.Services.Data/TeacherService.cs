namespace KyberKlass.Services.Data;

using Interfaces;
using KyberKlass.Data;
using KyberKlass.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.Admin;
using Web.ViewModels.Admin.User;

/// <summary>
///     Service class responsible for managing teachers.
/// </summary>

public class TeacherService : ITeacherService
{
    private readonly KyberKlassDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    /// Constructor for TeacherService.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="userManager">The user manager.</param>
    public TeacherService(KyberKlassDbContext dbContext, UserManager<ApplicationUser> userManager)
    {
        this._dbContext = dbContext;
        this._userManager = userManager;
    }

    /// <inheritdoc />
    public async Task<List<UserViewModel>?> AllAsync()
    {
        string teacherRoleName = "Teacher";

        var teacherRoleId = await this._dbContext
            .Roles
            .AsNoTracking()
            .Where(r => r.Name == teacherRoleName)
            .Select(r => r.Id)
            .FirstOrDefaultAsync();

        if (teacherRoleId != Guid.Empty)
        {
            // Retrieve all users who have the "Teacher" role
            List<ApplicationUser> teachers = await this._dbContext
                .Users
                .Where(user => this._dbContext
                    .UserRoles
                    .Any(userRole => userRole.UserId == user.Id && userRole.RoleId == teacherRoleId))
                //.Include(user => user.Role)
                .AsNoTracking()
                .ToListAsync();

            List<UserViewModel> teacherViewModels = teachers
                .Select(t => new UserViewModel
                {
                    Id = t.Id.ToString(),
                    Email = t.Email,
                    FullName = t.GetFullName(),
                    Role = teacherRoleName, // Hardcode the role as "Teacher" to avoid unnecessary role queries
                    IsActive = t.IsActive
                })
                .ToList();

            return teacherViewModels;
        }

        return null;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BasicViewModel>> GetUnassignedTeachersAsync()
    {
        IList<ApplicationUser> allTeachers = await this._userManager.GetUsersInRoleAsync("Teacher");

        if (allTeachers.Any() == false)
        {
            return Enumerable.Empty<BasicViewModel>();
        }

        List<Guid> assignedTeacherIds = await this._dbContext.Classrooms
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
        bool isAssigned = await this._dbContext
            .Classrooms
            .AsNoTracking()
            .AnyAsync(c => c.TeacherId == Guid.Parse(userId));

        return isAssigned;
    }
}