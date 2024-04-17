namespace KyberKlass.Services.Data;

using Interfaces;
using KyberKlass.Data;
using KyberKlass.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.Admin;

public class GuardianService : IGuardianService
{
    private readonly KyberKlassDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public GuardianService(KyberKlassDbContext dbContext, UserManager<ApplicationUser> userManager)
    {
        this._dbContext = dbContext;
        this._userManager = userManager;
    }

    public Task<Guardian?> GetByIdAsync(string id)
    {
        return this._dbContext
            .Guardians
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == Guid.Parse(id));
    }

    /// <inheritdoc />
    public async Task<bool> IsGuardianAssignedToStudentAsync(string userId)
    {
        bool isAssigned = await this._dbContext
            .Students
            .AsNoTracking()
            .AnyAsync(s => s.Guardian.Id == Guid.Parse(userId));

        return isAssigned;
    }

    public async Task<Guardian?> GetGuardianByUserIdAsync(string userId)
    {
        return await this._dbContext.Guardians
            .Include(g => g.ApplicationUser)
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Students.Any(s => s.Id == Guid.Parse(userId)));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BasicViewModel>> GetAllGuardiansAsync()
    {
        IList<ApplicationUser> guardians = await this._userManager.GetUsersInRoleAsync("Guardian");

        IEnumerable<BasicViewModel> guardianViewModels = guardians
            .Select(g => new BasicViewModel
            {
                Id = g.Id.ToString(),
                Name = g.GetFullName()
            })
            .ToArray();

        return guardianViewModels;
    }
}