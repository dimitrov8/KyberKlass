using KyberKlass.Data;
using KyberKlass.Data.Models;
using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Services.Data.Interfaces.Guardians;
using KyberKlass.Services.Data.Interfaces.Users;
using KyberKlass.Web.ViewModels.Admin;
using KyberKlass.Web.ViewModels.Admin.Student;
using KyberKlass.Web.ViewModels.Admin.User;
using Microsoft.EntityFrameworkCore;

namespace KyberKlass.Services.Data;
/// <summary>
///     Service class responsible for managing students.
/// </summary>
public class StudentService : IStudentService
{
    private readonly KyberKlassDbContext _dbContext;
    private readonly IUserService _userService;
    private readonly IGuardianService _guardianService;

    /// <summary>
    ///     Constructor for GuardianService.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="userService">The user service.</param>
    /// <param name="guardianService">The guardian service.</param>
    public StudentService(KyberKlassDbContext dbContext,
        IUserService userService,
        IGuardianService guardianService)
    {
        _dbContext = dbContext;
        _userService = userService;
        _guardianService = guardianService;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserViewModel>?> AllAsync()
    {
        string studentRoleName = "Student";

        Guid studentRoleId = await _dbContext
            .Roles
            .AsNoTracking()
            .Where(r => r.Name == studentRoleName)
            .Select(r => r.Id)
            .FirstOrDefaultAsync();

        if (studentRoleId != Guid.Empty)
        {
            // Retrieve all users who have the "Teacher" role
            List<ApplicationUser> students = await _dbContext
                .Users
                .Where(user => _dbContext
                    .UserRoles
                    .Any(userRole => userRole.UserId == user.Id && userRole.RoleId == studentRoleId))
                //.Include(user => user.Role)
                .AsNoTracking()
                .ToListAsync();

            List<UserViewModel> studentViewModels = students
                .Select(t => new UserViewModel
                {
                    Id = t.Id.ToString(),
                    Email = t.Email,
                    FullName = t.GetFullName(),
                    Role = studentRoleName, // Hardcode the role as "Student" to avoid unnecessary role queries
                    IsActive = t.IsActive
                })
                .ToList();

            return studentViewModels;
        }

        return null;
    }

    /// <inheritdoc />
    public Task<Student?> GetByIdASync(string id)
    {
        return _dbContext
            .Students
            .Include(s => s.Guardian)
            .FirstOrDefaultAsync(s => s.Id == Guid.Parse(id));
    }

    /// <inheritdoc />
    public async Task<StudentChangeGuardianViewModel> GetStudentChangeGuardianAsync(string userId)
    {
        UserDetailsViewModel? userDetails = await _userService.GetDetailsAsync(userId);
        IEnumerable<BasicViewModel> availableGuardians = await _guardianService.GetAllGuardiansAsync();

        StudentChangeGuardianViewModel viewModel = new()
        {
            UserDetails = userDetails,
            AvailableGuardians = availableGuardians
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