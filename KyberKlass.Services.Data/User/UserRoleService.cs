using KyberKlass.Data;
using KyberKlass.Data.Models;
using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Services.Data.Interfaces.Guardians;
using KyberKlass.Web.ViewModels.Admin;
using KyberKlass.Web.ViewModels.Admin.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KyberKlass.Services.Data.User
{
    public class UserRoleService : IUserRoleService
    {
        private readonly KyberKlassDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IUserService _userService;
        private readonly ISchoolService _schoolService;
        private readonly IGuardianService _guardianService;

        public UserRoleService(KyberKlassDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, IUserService userService, ISchoolService schoolService, IGuardianService guardianService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService;
            _schoolService = schoolService;
            _guardianService = guardianService;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<UserRolesViewModel>> GetAllRolesAsync()
        {
            IEnumerable<UserRolesViewModel> allRoles = await _roleManager
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

        /// <inheritdoc />
        public async Task<UserUpdateRoleViewModel?> GetForUpdateRoleAsync(string id)
        {
            ApplicationUser? user = await _userService.GetUserById(id);

            if (user == null)
            {
                return null;
            }

            string currentRoleName = await user.GetRoleAsync(_userManager);

            IEnumerable<UserRolesViewModel> availableRoles = await GetAllRolesAsync();

            UserUpdateRoleViewModel viewModel = new()
            {
                Id = user.Id.ToString(),
                FullName = user.GetFullName(),
                Email = user.Email,
                IsActive = user.IsActive,
                PreviousRoleName = currentRoleName,
                CurrentRoleName = currentRoleName,
                AvailableRoles = availableRoles
            };

            if (currentRoleName == "Guardian")
            {
                IEnumerable<BasicViewModel> students = await _dbContext
                    .Students
                    .Where(s => s.Guardian.Id == user.Id)
                    .Select(s => new BasicViewModel
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

        /// <inheritdoc />
        public async Task<string?> GetRoleNameByIdAsync(string id)
        {
            IdentityRole<Guid>? role = await _roleManager.FindByIdAsync(id);

            return role?.Name;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateRoleAsync(string userId, string roleId, string? guardianId, string? schoolId, string? classroomId)
        {
            ApplicationUser? user = await _userService.GetUserById(userId);
            if (user == null)
            {
                return false;
            }

            IdentityRole<Guid>? roleToUpdate = await _roleManager.FindByIdAsync(roleId); // Role to update to
            if (roleToUpdate == null)
            {
                return false;
            }

            string currentRoleName = await user.GetRoleAsync(_userManager); // Current user role

            if (currentRoleName == "Guardian") // If current user role is "Guardian"
            {
                Guardian? guardian = await _dbContext.Guardians.FindAsync(user.Id); // Get the guardian entity

                if (guardian != null && guardian.Students.Any()) // If the guardian exists and has students assigned
                {
                    return false; // Return false
                }
            }

            // If roleToUpdate is "Student" and ID'S required are not null
            if (roleToUpdate.Name == "Student" && guardianId != null && schoolId != null && classroomId != null)
            {
                // Validations for guardian school and classroom to assign to "Student"
                Guardian? guardianToAssign = await _guardianService.GetByIdAsync(guardianId);
                Web.ViewModels.Admin.School.SchoolDetailsViewModel? schoolToAssign = await _schoolService.GetByIdAsync(schoolId);
                bool classroomExistsInSchool = await _schoolService.ClassroomExistsInSchoolAsync(schoolId, classroomId);

                // If any of the validations fail return false
                if (guardianToAssign == null || schoolToAssign == null || classroomExistsInSchool == false)
                {
                    return false;
                }
            }

            // Start a transaction
            await using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                if (currentRoleName != "No Role Assigned")
                {
                    // Update user's role in the identity system
                    IdentityResult removeResult = await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));

                    if (removeResult.Succeeded == false)
                    {
                        // Roll back transaction if removing roles fails
                        await transaction.RollbackAsync();
                        return false;
                    }
                }

                IdentityResult addResult = await _userManager.AddToRoleAsync(user, roleToUpdate.Name);

                if (addResult.Succeeded == false)
                {
                    // Roll back transaction if adding roles fails
                    await transaction.RollbackAsync();
                    return false;
                }

                user.Role = roleToUpdate;

                // Update user role-related tables in the database
                // Remove user from the current role table based on the previous role
                switch (currentRoleName)
                {
                    case "Teacher":
                        Teacher? teacherToRemove = await _dbContext.Teachers.FindAsync(user.Id);
                        if (teacherToRemove != null)
                        {
                            _dbContext.Teachers.Remove(teacherToRemove);
                        }

                        break;
                    case "Guardian":
                        Guardian? guardianToRemove = await _dbContext.Guardians.FindAsync(user.Id);
                        if (guardianToRemove != null)
                        {
                            _dbContext.Guardians.Remove(guardianToRemove);
                        }

                        break;
                    case "Student":
                        Student? studentToRemove = await _dbContext.Students.FindAsync(user.Id);
                        if (studentToRemove != null)
                        {
                            _dbContext.Students.Remove(studentToRemove);
                        }

                        break;
                }

                // Update user role table based on the new role
                switch (roleToUpdate.Name)
                {
                    case "Teacher":
                        _dbContext.Teachers.Add(new Teacher { Id = user.Id });
                        break;
                    case "Guardian":
                        _dbContext.Guardians.Add(new Guardian { Id = user.Id });
                        break;
                    case "Student":
                        _dbContext.Students.Add(new Student
                        {
                            Id = user.Id,
                            GuardianId = Guid.Parse(guardianId!),
                            SchoolId = Guid.Parse(schoolId!),
                            ClassroomId = Guid.Parse(classroomId!)
                        });

                        break;
                }

                // Save changes to commit removal and addition of user from/to role tables
                await _dbContext.SaveChangesAsync();

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
    }
}
