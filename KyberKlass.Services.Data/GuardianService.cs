using KyberKlass.Data;
using KyberKlass.Data.Models;
using KyberKlass.Services.Data.Interfaces.Guardians;
using KyberKlass.Web.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KyberKlass.Services.Data;
/// <summary>
///     Service class responsible for managing guardians.
/// </summary>
public class GuardianService : IGuardianService
{
    private readonly KyberKlassDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    ///     Constructor for GuardianService.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="userManager">The user manager.</param>
    public GuardianService(KyberKlassDbContext dbContext, UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    /// <inheritdoc />
    public Task<Guardian?> GetByIdAsync(string id)
    {
        return _dbContext
            .Guardians
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == Guid.Parse(id));
    }

    /// <inheritdoc />
    public async Task<bool> IsGuardianAssignedToStudentAsync(string userId)
    {
        bool isAssigned = await _dbContext
            .Students
            .AsNoTracking()
            .AnyAsync(s => s.Guardian.Id == Guid.Parse(userId));

        return isAssigned;
    }

    /// <inheritdoc />
    public async Task<Guardian?> GetGuardianAssignedByUserIdAsync(string userId)
    {
        return await _dbContext.Guardians
            .Include(g => g.ApplicationUser)
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Students.Any(s => s.Id == Guid.Parse(userId)));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BasicViewModel>> GetAllGuardiansAsync()
    {
        IList<ApplicationUser> guardians = await _userManager.GetUsersInRoleAsync("Guardian");

        IEnumerable<BasicViewModel> guardianViewModels = guardians
            .Select(g => new BasicViewModel
            {
                Id = g.Id.ToString(),
                Name = g.GetFullName()
            })
            .ToArray();

        return guardianViewModels;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BasicViewModel>> GetStudentsAssignedToGuardianAsync(Guardian guardian)
    {
        return await _dbContext
            .Students
            .Where(s => s.Guardian.Id == guardian.Id)
            .Select(s => new BasicViewModel
            {
                Id = s.Id.ToString(),
                Name = s.ApplicationUser.GetFullName()
            })
          .AsNoTracking()
            .ToArrayAsync();
    }
}