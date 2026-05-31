#region

using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Services.Data.Interfaces.Guardians;
using KyberKlass.Services.Data.Interfaces.Users;
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
    private const string ControllerName = "User";

    private static string GetViewPath(string viewName)
    {
        return $"~/Views/Admin/{ControllerName}/{viewName}.cshtml";
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
        if (string.IsNullOrWhiteSpace(id))
        {
            return View("BadRequest400");
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
        if (string.IsNullOrWhiteSpace(id))
        {
            return View("BadRequest400");
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
        if (string.IsNullOrWhiteSpace(id))
        {
            return View("BadRequest400");
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
                    ? string.Format(SOFT_DELETION_SUCCESSFUL_MESSAGE, ControllerName, model.Id)
                    : string.Format(CHANGES_SUCCESSFULLY_APPLIED_MESSAGE, ControllerName, model.Id);
            }

            return RedirectToAction(nameof(All));
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, string.Format(EDIT_ERROR_MESSAGE, ControllerName.ToLower()));

            return View(GetViewPath(nameof(Edit)), model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> UpdateRole(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return View("BadRequest400");
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
        if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(roleId))
        {
            return View("BadRequest400");
        }

        try
        {
            UserUpdateRoleViewModel? userUpdateRoleViewModel = await userRoleService.GetForUpdateRoleAsync(id);

            string? currentRole = userUpdateRoleViewModel?.PreviousRoleName;
            string? roleToUpdateTo = await userRoleService.GetRoleNameByIdAsync(roleId);

            if (roleToUpdateTo != null && currentRole == "Teacher")
            {
                bool isTeacherAssignedToClassroom = await teacherService.IsTeacherAssignedToClassroomAsync(id);

                if (isTeacherAssignedToClassroom)
                {
                    TempData["ErrorMessage"] = string.Format(FAILED_TO_UPDATE_TEACHER_TO_OTHER_ROLE_MESSAGE, id);
                    return RedirectToAction(nameof(All));
                }
            }
            else if (roleToUpdateTo != null && currentRole == "Guardian")
            {
                bool isGuardianAssignedToStudent = await guardianService.IsGuardianAssignedToStudentAsync(id);

                if (isGuardianAssignedToStudent)
                {
                    TempData["ErrorMessage"] = string.Format(FAILED_TO_UPDATE_GUARDIAN_TO_OTHER_ROLE_MESSAGE, id);
                    return RedirectToAction(nameof(All));
                }
            }

            bool successfulRoleUpdate =
                await userRoleService.UpdateRoleAsync(id, roleId, guardianId, schoolId, classroomId);

            if (successfulRoleUpdate)
            {
                TempData["SuccessMessage"] = string.Format(ROLE_UPDATE_SUCCESS_MESSAGE, id);

                return RedirectToAction(nameof(All));
            }

            return View("BadRequest400");
        }
        catch (Exception)
        {
            return RedirectToAction(nameof(All));
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetGuardiansAndSchools()
    {
        IEnumerable<BasicViewModel> guardians = await guardianService.GetAllGuardiansAsync();
        IEnumerable<BasicViewModel> schools = await schoolService.BasicAllAsync();

        var data = new { Guardians = guardians, Schools = schools };

        return Json(data);
    }
}
