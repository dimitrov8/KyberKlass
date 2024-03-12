namespace KyberKlass.Services.Data;

using System.Globalization;
using Interfaces;
using KyberKlass.Data;
using KyberKlass.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.ViewModels.Admin.User;

public class UserService : IUserService
{
    private readonly KyberKlassDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public UserService(KyberKlassDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
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
    /// Retrieves details of a user asynchronously based on the provided ID.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve details for.</param>
    /// <returns>
    /// A user details view model representing the specified user, or null if the user is not found.
    /// </returns>
    public async Task<UserDetailsViewModel?> GetUserDetailsAsync(string id)
    {
        var user = await this._dbContext
            .Users
            .FirstOrDefaultAsync(u => u.Id.ToString() == id); // Retrieve the user from the database based on the provided ID

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

        return viewModel;
    }

    /// <summary>
    /// Retrieves a user asynchronously for updating their role based on the provided ID.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve for updating their role.</param>
    /// <returns>
    /// A user update role view model representing the specified user, or null if the user is not found.
    /// </returns>
    public async Task<UserUpdateRoleViewModel?> GetUserForUpdateRoleAsync(string id)
    {
        var user = await this._dbContext
            .Users
            .FirstOrDefaultAsync(u => u.Id.ToString() == id); // Retrieve the user from the database based on the provided ID

        if (user == null)
        {
            return null;  // Return null if the user is not found
        }

        IEnumerable<UserRolesViewModel> availableRoles = await this.GetAllRolesAsync(); // Retrieve all available roles asynchronously

        // Create a view model for updating user role
        var viewModel = new UserUpdateRoleViewModel
        {
            Id = user.Id.ToString(),
            FullName = user.GetFullName(),
            Email = user.Email,
            IsActive = user.IsActive,
            CurrentRoleName = await user.GetRoleAsync(this._userManager), // Retrieve the user's current role asynchronously
            AvailableRoles = availableRoles // Assign available roles to the view model
        };

        return viewModel;
    }

    public async Task<bool> IsNotNullOrEmptyInputAsync(string id, UserViewModel? model)
    {
        if (string.IsNullOrEmpty(id))
        {
            return false;
        }

        if (model != null && string.IsNullOrEmpty(model.Id))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Updates the role of a user identified by the provided ID.
    /// If the user's current role matches the selected role, no changes are made.
    /// Otherwise, the user is removed from all current roles and added to the new role.
    /// The Role property of ApplicationUser is updated if necessary, and changes are saved to the database.
    /// Returns the name of the updated role if successful; otherwise, returns null.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="roleId">The ID of the new role.</param>
    /// <returns>The name of the updated role if successful; otherwise, null.</returns>
    public async Task<string?> UpdateRoleAsync(string id, string roleId)
    {
        var user = await this._dbContext
            .Users
            .FirstOrDefaultAsync(u => u.Id.ToString() == id);

        if (user == null)
        {
            return null;
        }

        // Use the GetRoleAsync method from ApplicationUser to get the user's role
        IdentityRole<Guid>? role = await this._roleManager
            .Roles
            .FirstOrDefaultAsync(r => r.Id.ToString() == roleId);

        if (role == null)
        {
            return null;
        }

        // Get the user's current role using your GetRoleAsync method
        string? currentRoleName = await user.GetRoleAsync(this._userManager);

        if (currentRoleName == role.Name)
        {
            return role.Name; // User already has the selected role, no need to update
        }

        // Remove the user from all current roles
        var removeResult = await this._userManager.RemoveFromRolesAsync(user, await this._userManager.GetRolesAsync(user));

        if (!removeResult.Succeeded)
        {
            return null; // Return null if unable to remove user from current roles
        }

        // Add the user to the new role
        var addResult = await this._userManager.AddToRoleAsync(user, role.Name);

        if (addResult.Succeeded)
        {
            // Update the Role property of ApplicationUser (if necessary)
            user.Role = role;

            // Save changes to update the Role property in the database
            await this._dbContext.SaveChangesAsync();

            return role.Name;
        }

        return null; // Return null if unable to add role
    }

    /// <summary>
    /// Retrieves all roles from the database and returns them as an enumerable collection of UserRolesViewModel objects.
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
}