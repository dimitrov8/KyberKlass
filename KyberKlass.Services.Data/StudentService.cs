#region

using KyberKlass.Data;
using KyberKlass.Data.Models;
using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Services.Data.Interfaces.Guardians;
using KyberKlass.Services.Data.Interfaces.Users;
using KyberKlass.Web.ViewModels.Admin;
using KyberKlass.Web.ViewModels.Admin.Student;
using KyberKlass.Web.ViewModels.Admin.User;

using Microsoft.EntityFrameworkCore;

#endregion

namespace KyberKlass.Services.Data;

/// <summary>
///     Service class responsible for managing students.
/// </summary>
/// <remarks>
///     Constructor for GuardianService.
/// </remarks>
/// <param name="dbContext">The database context.</param>
/// <param name="userService">The user service.</param>
/// <param name="guardianService">The guardian service.</param>
public class StudentService(KyberKlassDbContext dbContext,
    IUserService userService,
    IGuardianService guardianService) : IStudentService
{
    private readonly KyberKlassDbContext _dbContext = dbContext;
    private readonly IGuardianService _guardianService = guardianService;
    private readonly IUserService _userService = userService;

    /// <inheritdoc />
    public async Task<IEnumerable<UserViewModel>> AllAsync(string? searchTerm = null)
    {
        const string studentRoleName = "Student";

        Guid studentRoleId = await _dbContext
            .Roles
            .AsNoTracking()
            .Where(predicate: r => r.Name == studentRoleName)
            .Select(selector: r => r.Id)
            .FirstOrDefaultAsync();

        if (studentRoleId == Guid.Empty)
        {
            return Enumerable.Empty<UserViewModel>();
        }

        IQueryable<ApplicationUser> query = _dbContext
            .Users
            .Where(predicate: user => _dbContext
                .UserRoles
                .Any(userRole => userRole.UserId == user.Id && userRole.RoleId == studentRoleId))
            .AsNoTracking();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            string term = searchTerm.ToLower();
            query = query.Where(predicate: u =>
                u.FirstName.ToLower().Contains(term) ||
                u.LastName.ToLower().Contains(term) ||
                u.Email.ToLower().Contains(term));
        }

        List<ApplicationUser> students = await query.ToListAsync();

        List<UserViewModel> studentViewModels = students
            .Select(selector: t => new UserViewModel
            {
                Id = t.Id.ToString(),
                Email = t.Email,
                FullName = t.GetFullName(),
                Role = studentRoleName // Hardcode the role as "Student" to avoid unnecessary role queries
            })
            .ToList();

        return studentViewModels;
    }

    /// <inheritdoc />
    public Task<Student?> GetByIdASync(string id)
    {
        return _dbContext
            .Students
            .Include(navigationPropertyPath: s => s.Guardian)
            .FirstOrDefaultAsync(predicate: s => s.Id == Guid.Parse(id));
    }

    /// <inheritdoc />
    public async Task<StudentChangeGuardianViewModel> GetStudentChangeGuardianAsync(string userId)
    {
        UserDetailsViewModel? userDetails = await _userService.GetDetailsAsync(userId);
        IEnumerable<BasicViewModel> availableGuardians = await _guardianService.GetAllGuardiansAsync();

        StudentChangeGuardianViewModel viewModel = new()
        {
            UserDetails = userDetails, AvailableGuardians = availableGuardians
        };

        return viewModel;
    }

    /// <inheritdoc />
    public async Task<bool> StudentChangeGuardianAsync(string userId, string guardianId)
    {
        Student? student = await GetByIdASync(userId);
        Guardian? newGuardian = await _guardianService.GetByIdAsync(guardianId);

        if (student == null || newGuardian == null)
        {
            return false;
        }

        Guid previousGuardianId = student.Guardian.Id;

        if (previousGuardianId == newGuardian.Id)
        {
            return false;
        }

        student.GuardianId = Guid.Empty;
        student.GuardianId = newGuardian.Id;

        await _dbContext.SaveChangesAsync();

        return true;
    }
}