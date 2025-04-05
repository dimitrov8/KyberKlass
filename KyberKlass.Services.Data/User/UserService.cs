using KyberKlass.Data;
using KyberKlass.Data.Models;
using KyberKlass.Services.Data.Interfaces.Guardians;
using KyberKlass.Services.Data.Interfaces.Users;
using KyberKlass.Web.ViewModels.Admin;
using KyberKlass.Web.ViewModels.Admin.Guardian;
using KyberKlass.Web.ViewModels.Admin.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static KyberKlass.Common.FormattingConstants;

namespace KyberKlass.Services.Data.User;
/// <summary>
///     Service class responsible for managing users.
/// </summary>
public class UserService : IUserService
{
    private readonly KyberKlassDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IGuardianService _guardianService;

    /// <summary>
    ///     Constructor for UserService.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="userManager">The user manager.</param>
    /// <param name="guardianService">The guardian service.</param>
    public UserService(KyberKlassDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        IGuardianService guardianService)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _guardianService = guardianService;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserViewModel>> AllAsync()
    {
        var usersWithRoles = await _dbContext
            .Users
            .Select(user => new
            {
                User = user,
                Roles = _dbContext
                    .UserRoles
                    .Where(ur => ur.UserId == user.Id)
                    .Join(_dbContext.Roles,
                        ur => ur.RoleId,
                        role => role.Id,
                        (ur, role) => role.Name)
                    .ToArray()
            })
            .AsNoTracking()
            .ToArrayAsync();

        IEnumerable<UserViewModel> userViewModels = usersWithRoles
            .Select(u => new UserViewModel
            {
                Id = u.User.Id.ToString(),
                FullName = u.User.GetFullName(),
                Email = u.User.Email,
                Role = u.Roles.FirstOrDefault() ?? "No Role Assigned",
                IsActive = u.User.IsActive
            })
            .ToArray();

        return userViewModels;
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
                IEnumerable<BasicViewModel> guardianStudents = await _guardianService.GetStudentsAssignedToGuardianAsync(guardian);

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
                IEnumerable<BasicViewModel> students = await _guardianService.GetStudentsAssignedToGuardianAsync(guardian);

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