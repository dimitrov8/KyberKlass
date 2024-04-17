namespace KyberKlass.Services.Data;

using Interfaces;
using KyberKlass.Data;
using KyberKlass.Data.Models;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.Admin;
using Web.ViewModels.Admin.Student;
using Web.ViewModels.Admin.User;

public class StudentService : IStudentService
{
    private readonly KyberKlassDbContext _dbContext;
    private readonly IUserService _userService;
    private readonly IGuardianService _guardianService;

    public StudentService(KyberKlassDbContext dbContext,
        IUserService userService,
        IGuardianService guardianService)
    {
        this._dbContext = dbContext;
        this._userService = userService;
        this._guardianService = guardianService;
    }

    public async Task<IEnumerable<UserViewModel>?> AllAsync()
    {
        string studentRoleName = "Student";

        var studentRoleId = await this._dbContext
            .Roles
            .AsNoTracking()
            .Where(r => r.Name == studentRoleName)
            .Select(r => r.Id)
            .FirstOrDefaultAsync();

        if (studentRoleId != Guid.Empty)
        {
            // Retrieve all users who have the "Teacher" role
            List<ApplicationUser> students = await this._dbContext
                .Users
                .Where(user => this._dbContext
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

    public Task<Student?> GetByIdASync(string id)
    {
        return this._dbContext
            .Students
            .Include(s => s.Guardian)
            .FirstOrDefaultAsync(s => s.Id == Guid.Parse(id));
    }

    public async Task<StudentChangeGuardianViewModel> GetStudentChangeGuardianAsync(string userId)
    {
        var userDetails = await this._userService.GetDetailsAsync(userId);
        IEnumerable<BasicViewModel> availableGuardians = await this._guardianService.GetAllGuardiansAsync();

        var viewModel = new StudentChangeGuardianViewModel
        {
            UserDetails = userDetails,
            AvailableGuardians = availableGuardians
        };

        return viewModel;
    }

    public async Task<bool> StudentChangeGuardianAsync(string userId, string guardianId)
    {
        var student = await this.GetByIdASync(userId);
        var newGuardian = await this._guardianService.GetByIdAsync(guardianId);

        if (student == null || newGuardian == null)
        {
            return false;
        }

        var previousGuardianId = student.Guardian.Id;

        if (previousGuardianId == newGuardian.Id)
        {
            return false;
        }

        student.GuardianId = Guid.Empty;
        student.GuardianId = newGuardian.Id;

        await this._dbContext.SaveChangesAsync();

        return true;
    }
}