namespace KyberKlass.Services.Data;

using System.Globalization;
using Interfaces;
using KyberKlass.Data;
using KyberKlass.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.Admin.Guardian;
using Web.ViewModels.Admin.User;
using static Common.FormattingConstants;

public class UserService : IUserService
{
    private readonly KyberKlassDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public UserService(KyberKlassDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        this._dbContext = dbContext;
        this._userManager = userManager;
        this._roleManager = roleManager;
    }

    public async Task<List<UserViewModel>> AllAsync()
    {
        var usersWithRoles = await this._dbContext.Users
            .Select(user => new
            {
                User = user,
                Roles = this._dbContext.UserRoles
                    .Where(ur => ur.UserId == user.Id)
                    .Join(this._dbContext.Roles,
                        ur => ur.RoleId,
                        role => role.Id,
                        (ur, role) => role.Name)
                    .ToList()
            })
            .ToListAsync();

        List<UserViewModel> userViewModels = usersWithRoles
            .Select(u => new UserViewModel
            {
                Id = u.User.Id.ToString(),
                FullName = u.User.GetFullName(),
                Email = u.User.Email,
                Role = u.Roles.FirstOrDefault() ?? "No Role Assigned",
                IsActive = u.User.IsActive
            })
            .ToList();

        return userViewModels; // TODO Make this method better
    }

    /// <summary>
    ///     Retrieves details of a user asynchronously based on the provided ID.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve details for.</param>
    /// <returns>
    ///     A user details view model representing the specified user, or null if the user is not found.
    /// </returns>
    public async Task<UserDetailsViewModel?> GetDetailsAsync(string id)
    {
        var user = await this.GetUserById(id); // Retrieve the user from the database based on the provided ID

        if (user == null)
        {
            return null; // Return null if the user is not found
        }

        // Map user details to a view model
        var viewModel = new UserDetailsViewModel
        {
            Id = user.Id.ToString(),
            FullName = user.GetFullName(),
            BirthDate = user.GetBirthDate(),
            Address = user.Address,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            Role = await user.GetRoleAsync(this._userManager), // Retrieve the user's role asynchronously
            IsActive = user.GetStatus()
        };

        if (viewModel.Role == "Student")
        {
            var guardian = await this._dbContext
                .Guardians
                .Include(g => g.ApplicationUser)
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Students.Any(s => s.Id == user.Id));

            if (guardian != null)
            {
                viewModel.Guardian = new GuardianViewModel
                {
                    FullName = guardian.ApplicationUser.GetFullName(),
                    Address = guardian.ApplicationUser.Address,
                    Email = guardian.ApplicationUser.Email,
                    PhoneNumber = guardian.ApplicationUser.PhoneNumber
                };
            }
        }
        else if (viewModel.Role == "Guardian")
        {
            IEnumerable<UserBasicViewModel> students = await this._dbContext
                .Students
                .Where(s => s.Guardian.Id == user.Id)
                .Select(s => new UserBasicViewModel
                {
                    Id = s.Id.ToString(),
                    Name = s.ApplicationUser.GetFullName()
                })
                .AsNoTracking()
                .ToArrayAsync();

            viewModel.Students = students;
        }

        return viewModel;
    }

    /// <summary>
    ///     Retrieves a user asynchronously for updating their role based on the provided ID.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve for updating their role.</param>
    /// <returns>
    ///     A user update role view model representing the specified user, or null if the user is not found.
    /// </returns>
    public async Task<UserUpdateRoleViewModel?> GetForUpdateRoleAsync(string id)
    {
        var user = await this.GetUserById(id); // Retrieve the user from the database based on the provided ID

        if (user == null)
        {
            return null; // Return null if the user is not found
        }

        var currentRoleName = await user.GetRoleAsync(this._userManager);

  

        IEnumerable<UserRolesViewModel> availableRoles = await this.GetAllRolesAsync(); // Retrieve all available roles asynchronously

        // Create a view model for updating user role
        var viewModel = new UserUpdateRoleViewModel
        {
            Id = user.Id.ToString(),
            FullName = user.GetFullName(),
            Email = user.Email,
            IsActive = user.IsActive,
            PreviousRoleName = currentRoleName,
            CurrentRoleName = currentRoleName,
            AvailableRoles = availableRoles, // Assign available roles to the view model
        };
        if (currentRoleName == "Guardian")
        {
            IEnumerable<UserBasicViewModel> students = await this._dbContext.Students
                .Where(s => s.Guardian.Id == user.Id)
                .Select(s => new UserBasicViewModel
                {
                    Id = s.Id.ToString(),
                    Name = s.ApplicationUser.GetFullName()
                })
                .AsNoTracking()
                .ToArrayAsync();

            viewModel.Students = students;
        }

        return viewModel;
    }

    public async Task<IEnumerable<UserBasicViewModel>> GetAllGuardiansAsync()
    {
        IList<ApplicationUser> guardians = await this._userManager.GetUsersInRoleAsync("Guardian");

        List<UserBasicViewModel> guardianViewModels = guardians
            .Select(g => new UserBasicViewModel
            {
                Id = g.Id.ToString(),
                Name = g.GetFullName()
            })
            .ToList();

        return guardianViewModels;
    }

    public async Task<bool> IsTeacherAssignedToClassroomAsync(string userId)
    {
        // Check if the user with the provided userId is assigned to any classroom as a teacher
        var isAssigned = await this._dbContext.Classrooms
            .AnyAsync(c => c.TeacherId == Guid.Parse(userId));

        return isAssigned;
    }

    public async Task<bool> IsGuardianAssignedToStudentAsync(string userId)
    {
        var isAssigned = await this._dbContext.Students
            .AnyAsync(s => s.Guardian.Id == Guid.Parse(userId));

        return isAssigned;
    }

    public async Task<bool> UpdateRoleAsync(string userId, string roleId, string? guardianId, string? classroomId)
    {
        var user = await this.GetUserById(userId);
        if (user == null)
            return false;

        IdentityRole<Guid>? role = await this._roleManager.FindByIdAsync(roleId);
        if (role == null)
            return false;

        string currentRoleName = await user.GetRoleAsync(this._userManager);
        if (currentRoleName == role.Name)
            return true;

        if (currentRoleName == "Guardian")
        {
            var guardian = await this._dbContext.Guardians.FindAsync(user.Id);

            if (guardian != null && guardian.Students.Any())
            {
                return false;
            }
        }

        // Start a transaction
        await using var transaction = await this._dbContext.Database.BeginTransactionAsync();

        try
        {
            // Update user's role in the identity system
            var removeResult = await this._userManager.RemoveFromRolesAsync(user, await this._userManager.GetRolesAsync(user));
            if (!removeResult.Succeeded)
            {
                // Roll back transaction if removing roles fails
                await transaction.RollbackAsync();
                return false;
            }

            var addResult = await this._userManager.AddToRoleAsync(user, role.Name);
            if (!addResult.Succeeded)
            {
                // Roll back transaction if adding roles fails
                await transaction.RollbackAsync();
                return false;
            }

            user.Role = role;

            // Update user role-related tables in the database
            // Remove user from the current role table based on the previous role
            switch (currentRoleName)
            {
                case "Teacher":
                    var teacherToRemove = await this._dbContext.Teachers.FindAsync(user.Id);
                    if (teacherToRemove != null)
                        this._dbContext.Teachers.Remove(teacherToRemove);
                    break;
                case "Guardian":
                    var guardianToRemove = await this._dbContext.Guardians.FindAsync(user.Id);
                    if (guardianToRemove != null)
                        this._dbContext.Guardians.Remove(guardianToRemove);
                    break;
                case "Student":
                    var studentToRemove = await this._dbContext.Students.FindAsync(user.Id);
                    if (studentToRemove != null)
                        this._dbContext.Students.Remove(studentToRemove);
                    break;
            }

            // Update user role table based on the new role
            switch (role.Name)
            {
                case "Teacher":
                    this._dbContext.Teachers.Add(new Teacher { Id = user.Id });
                    break;
                case "Guardian":
                    this._dbContext.Guardians.Add(new Guardian { Id = user.Id });
                    break;
                case "Student":
                    this._dbContext.Students.Add(new Student
                    {
                        Id = user.Id,
                        GuardianId = Guid.Parse(guardianId!),
                        ClassroomId = Guid.Parse(classroomId!),
                    });
                    break;
            }

            // Save changes to commit removal and addition of user from/to role tables
            await this._dbContext.SaveChangesAsync();

            // Commit the transaction
            await transaction.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            // Roll back transaction in case of any exception
            await transaction.RollbackAsync();
            return false;
        }
    }

    /// <summary>
    ///     Retrieves all roles from the database and returns them as an enumerable collection of UserRolesViewModel objects.
    /// </summary>
    /// <returns>An enumerable collection of UserRolesViewModel objects representing all roles.</returns>
    public async Task<IEnumerable<UserRolesViewModel>> GetAllRolesAsync()
    {
        IEnumerable<UserRolesViewModel> allRoles = await this._roleManager
            .Roles
            .Select(r => new UserRolesViewModel
            {
                Id = r.Id.ToString(),
                Name = r.Name
            })
            .AsNoTracking()
            .ToArrayAsync(); // Retrieve all roles from the RoleManager and map them to UserRolesViewModel objects

        return allRoles;
    }

    public async Task<UserEditFormModel?> EditAsync(string id, UserEditFormModel model)
    {
        var user = await this.GetUserById(id);

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

        await this._dbContext.SaveChangesAsync();

        return model;
    }

    public async Task<UserEditFormModel?> GetForEditAsync(string id)
    {
        var user = await this.GetUserById(id);

        if (user == null)
        {
            return null;
        }

        var viewModel = new UserEditFormModel
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

    public async Task<ApplicationUser?> GetUserById(string id)
    {
        return await this._dbContext
            .Users
            .FindAsync(Guid.Parse(id));
    }

    public async Task<string?> GetRoleNameByIdAsync(string id)
    {
        var role = await this._roleManager.FindByIdAsync(id);
        var roleName = role.Name;

        return roleName;
    }
}