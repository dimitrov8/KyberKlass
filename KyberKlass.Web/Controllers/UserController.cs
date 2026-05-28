#region

using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Services.Data.Interfaces.Guardians;
using KyberKlass.Services.Data.Interfaces.Users;
using KyberKlass.Web.Infrastructure.Extensions;
using KyberKlass.Web.ViewModels.Admin;
using KyberKlass.Web.ViewModels.Admin.User;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static KyberKlass.Common.CustomMessageConstants.Common;
using static KyberKlass.Common.CustomMessageConstants.User;

#endregion

namespace KyberKlass.Web.Controllers;

[Authorize(Roles = "Admin")]
public class UserController(
    IUserService userService,
    IUserRoleService userRoleService,
    ISchoolService schoolService,
    ITeacherService teacherService,
    IGuardianService guardianService)
    : Controller
{
    private const string CONTROLLER_NAME = "User";

    private static string GetViewPath(string viewName)
    {
        return $"~/Views/Admin/User/{viewName}.cshtml";
    }

    [HttpGet]
    public async Task<IActionResult> All(string? searchTerm, string? roleFilter)
    {
        IEnumerable<UserViewModel> users = await userService.AllAsync(searchTerm, roleFilter);

        if (Request.Headers.XRequestedWith == "XMLHttpRequest")
        {
            return PartialView(GetViewPath("_AllPartial"), users);
        }

        ViewData["searchTerm"] = searchTerm;
        ViewData["roleFilter"] = roleFilter;
        return View(GetViewPath(nameof(All)), users);
    }

    [HttpGet]
    public async Task<IActionResult> Details(string id)
    {
        bool isValidInput = ValidationExtensions.IsNotNullOrEmptyInput<string>(id, null);

        if (!isValidInput)
        {
            return View("BadRequest400");
            //return BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            UserDetailsViewModel? userDetailsViewModel = await userService.GetDetailsAsync(id);

            return userDetailsViewModel == null
                ? View("NotFound404")
                : View(GetViewPath(nameof(Details)), userDetailsViewModel);
        }
        catch (Exception)
        {
            return RedirectToAction(nameof(All));
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        bool isValidInput = ValidationExtensions.IsNotNullOrEmptyInput<string>(id, null);

        if (!isValidInput)
        {
            return View("BadRequest400");
            //return BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            UserEditFormModel? userViewModel = await userService.GetForEditAsync(id);

            return userViewModel == null ? View("NotFound404") : View(GetViewPath(nameof(Edit)), userViewModel);
        }
        catch (Exception)
        {
            return RedirectToAction(nameof(All));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(string id, UserEditFormModel model)
    {
        bool isValidInput = ValidationExtensions.IsNotNullOrEmptyInput<string>(id, null);

        if (!isValidInput)
        {
            return View("BadRequest400");
            //return BadRequest(INVALID_INPUT_MESSAGE);
        }

        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = UNABLE_TO_SAVE_CHANGES_MESSAGE;

            return View(GetViewPath(nameof(Edit)), model);
        }

        try
        {
            UserEditFormModel? userToEdit = await userService.EditAsync(id, model);

            if (userToEdit != null)
            {
                TempData["SuccessMessage"] = !model.IsActive
                    ? string.Format(SOFT_DELETION_SUCCESSFUL_MESSAGE, CONTROLLER_NAME, model.Id)
                    : string.Format(CHANGES_SUCCESSFULLY_APPLIED_MESSAGE, CONTROLLER_NAME, model.Id);
            }

            return RedirectToAction(nameof(All));
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, string.Format(EDIT_ERROR_MESSAGE, CONTROLLER_NAME.ToLower()));

            return View(GetViewPath(nameof(Edit)), model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> UpdateRole(string id)
    {
        bool isValidInput = ValidationExtensions.IsNotNullOrEmptyInput<string>(id, null);

        if (!isValidInput)
        {
            return View("BadRequest400");
            //return BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            UserUpdateRoleViewModel? userUpdateRoleViewModel = await userRoleService.GetForUpdateRoleAsync(id);

            return userUpdateRoleViewModel == null
                ? View("NotFound404")
                : View(GetViewPath(nameof(UpdateRole)), userUpdateRoleViewModel);
        }
        catch (Exception)
        {
            return RedirectToAction(nameof(All));
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRoleConfirmed(
    string id,
    string roleId,
    string? guardianId,
    string? schoolId,
    string? classroomId)
    {
        bool isValidInput = ValidationExtensions.IsNotNullOrEmptyInput<string>(id, null) &&
                            ValidationExtensions.IsNotNullOrEmptyInput<string>(roleId, null);

        if (!isValidInput)
        {
            return View("BadRequest400");
            //return BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            UserUpdateRoleViewModel?
                userUpdateRoleViewModel =
                    await userRoleService.GetForUpdateRoleAsync(id); // Retrieve the user viewmodel

            string? currentRole =
                userUpdateRoleViewModel?.PreviousRoleName; // Gets the name of the role before the update
            string? roleToUpdateTo =
                await userRoleService
                    .GetRoleNameByIdAsync(roleId); // Get the name of the role which we want to update to

            if (roleToUpdateTo != null &&
                currentRole == "Teacher") // If user wants to update to a valid role and his current role is "Teacher"
            {
                // Checks if teacher is assigned to a classroom
                bool isTeacherAssignedToClassroom = await teacherService.IsTeacherAssignedToClassroomAsync(id);

                if (isTeacherAssignedToClassroom) // If teacher is assigned to a classroom
                {
                    // Display a message to change the classroom teacher
                    TempData["ErrorMessage"] = string.Format(FAILED_TO_UPDATE_TEACHER_TO_OTHER_ROLE_MESSAGE, id);
                    return RedirectToAction(nameof(All));
                }
            }
            else if
                (roleToUpdateTo != null &&
                 currentRole ==
                 "Guardian") // If user wants to update to a valid role and his current role is "Guardian"
            {
                // Checks if guardian is assigned to any student
                bool isGuardianAssignedToStudent = await guardianService.IsGuardianAssignedToStudentAsync(id);

                if (isGuardianAssignedToStudent) // If guardian is assigned to any student
                {
                    // Display a message to change the student(s) guardian
                    TempData["ErrorMessage"] = string.Format(FAILED_TO_UPDATE_GUARDIAN_TO_OTHER_ROLE_MESSAGE, id);
                    return RedirectToAction(nameof(All));
                }
            }

            bool successfulRoleUpdate =
                await userRoleService.UpdateRoleAsync(id, roleId, guardianId, schoolId, classroomId);

            if (successfulRoleUpdate)
            {
                TempData["SuccessMessage"] = string.Format(ROLE_UPDATE_SUCCESS_MESSAGE, id);

                return RedirectToAction(roleToUpdateTo switch
                {
                    _ => nameof(All)
                });
            }

            return View("BadRequest400");
            //return BadRequest(string.Format(ROLE_UPDATE_FAILED_MESSAGE, id));
        }
        catch (Exception)
        {
            return RedirectToAction(nameof(All)); // Can return custom error view
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetGuardiansAndSchools()
    {
        IEnumerable<BasicViewModel> guardians = await guardianService.GetAllGuardiansAsync(); // Fetch guardians
        IEnumerable<BasicViewModel> schools = await schoolService.BasicAllAsync(); // Fetch schools

        var data = new { Guardians = guardians, Schools = schools };

        return Json(data);
    }
}