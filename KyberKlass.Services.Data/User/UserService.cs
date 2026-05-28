#region

using System.Globalization;

using KyberKlass.Data;
using KyberKlass.Data.Models;
using KyberKlass.Services.Data.Interfaces.Guardians;
using KyberKlass.Services.Data.Interfaces.Users;
using KyberKlass.Web.ViewModels.Admin;
using KyberKlass.Web.ViewModels.Admin.Guardian;
using KyberKlass.Web.ViewModels.Admin.User;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using static KyberKlass.Common.FormattingConstants;

#endregion

namespace KyberKlass.Services.Data.User;

/// <summary>
///     Service class responsible for managing users.
/// </summary>
/// <remarks>
///     Constructor for UserService.
/// </remarks>
/// <param name="dbContext">The database context.</param>
/// <param name="userManager">The user manager.</param>
/// <param name="guardianService">The guardian service.</param>
public class UserService(KyberKlassDbContext dbContext,
    UserManager<ApplicationUser> userManager,
    IGuardianService guardianService) : IUserService
{
    private readonly KyberKlassDbContext _dbContext = dbContext;
    private readonly IGuardianService _guardianService = guardianService;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    /// <inheritdoc />
    public async Task<IEnumerable<UserViewModel>> AllAsync(string? searchTerm = null, string? roleFilter = null)
    {
        var query = _dbContext.Users
            .Where(predicate: u => u.IsActive)
            .Select(selector: u => new
            {
                User = u,
                RoleName = _dbContext.UserRoles
                    .Where(ur => ur.UserId == u.Id)
                    .Join(_dbContext.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                    .FirstOrDefault()
            });

        if (!string.IsNullOrEmpty(searchTerm))
        {
            string term = searchTerm.ToLower();
            query = query.Where(predicate: x =>
                x.User.FirstName.ToLower().Contains(term) ||
                x.User.LastName.ToLower().Contains(term) ||
                x.User.Email.ToLower().Contains(term));
        }

        if (!string.IsNullOrEmpty(roleFilter))
        {
            query = roleFilter.Equals("NoRoleAssigned")
                ? query.Where(predicate: x => x.RoleName == null)
                : query.Where(predicate: x => x.RoleName != null && x.RoleName.ToLower() == roleFilter.ToLower());
        }

        var results = await query.ToListAsync();

        return results.Select(selector: x => new UserViewModel
        {
            Id = x.User.Id.ToString(),
            Email = x.User.Email,
            FullName = x.User.GetFullName(),
            Role = x.RoleName ?? "No Role Assigned"
        });
    }

    /// <inheritdoc />
    public async Task<UserDetailsViewModel?> GetDetailsAsync(string id)
    {
        ApplicationUser? user = await GetUserById(id);

        if (user == null)
        {
            return null;
        }

        UserDetailsViewModel viewModel = new()
        {
            Id = user.Id.ToString(),
            FullName = user.GetFullName(),
            BirthDate = user.GetBirthDate(),
            Address = user.Address,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            Role = await user.GetRoleAsync(_userManager), // Retrieve the user's role asynchronously
            IsActive = user.GetStatus()
        };

        if (viewModel.Role == "Student")
        {
            Guardian? guardian = await _guardianService.GetGuardianAssignedByUserIdAsync(viewModel.Id);

            if (guardian != null)
            {
                IEnumerable<BasicViewModel> guardianStudents =
                    await _guardianService.GetStudentsAssignedToGuardianAsync(guardian);

                viewModel.Guardian = new GuardianViewModel
                {
                    Id = guardian.Id.ToString(),
                    FullName = guardian.ApplicationUser.GetFullName(),
                    Address = guardian.ApplicationUser.Address,
                    Email = guardian.ApplicationUser.Email,
                    PhoneNumber = guardian.ApplicationUser.PhoneNumber,
                    Students = guardianStudents
                };
            }
        }
        else if (viewModel.Role == "Guardian")
        {
            Guardian? guardian = await _guardianService.GetGuardianAssignedByUserIdAsync(viewModel.Id);

            if (guardian != null)
            {
                IEnumerable<BasicViewModel> students =
                    await _guardianService.GetStudentsAssignedToGuardianAsync(guardian);

                viewModel.Students = students;
            }
        }

        return viewModel;
    }

    /// <inheritdoc />
    public async Task<UserEditFormModel?> EditAsync(string id, UserEditFormModel model)
    {
        ApplicationUser? user = await GetUserById(id);

        if (user == null)
        {
            return null;
        }

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.BirthDate = DateTime.ParseExact(model.BirthDate, BIRTH_DATE_FORMAT, CultureInfo.InvariantCulture);
        user.Address = model.Address;
        user.PhoneNumber = model.PhoneNumber;
        user.Email = model.Email.ToLower();
        user.NormalizedEmail = model.Email.ToUpper();
        user.IsActive = model.IsActive;

        await _dbContext.SaveChangesAsync();

        return model;
    }

    /// <inheritdoc />
    public async Task<UserEditFormModel?> GetForEditAsync(string id)
    {
        ApplicationUser? user = await GetUserById(id);

        if (user == null)
        {
            return null;
        }

        UserEditFormModel viewModel = new()
        {
            Id = user.Id.ToString(),
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthDate = user.GetBirthDate(),
            Address = user.Address,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            IsActive = user.IsActive
        };

        return viewModel;
    }

    /// <inheritdoc />
    public async Task<ApplicationUser?> GetUserById(string id)
    {
        return await _dbContext
            .Users
            .FindAsync(Guid.Parse(id));
    }
}