using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Services.Data.Interfaces.Guardians;
using KyberKlass.Web.Infrastructure.Extensions;
using KyberKlass.Web.ViewModels.Admin;
using KyberKlass.Web.ViewModels.Admin.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static KyberKlass.Common.CustomMessageConstants.Common;
using static KyberKlass.Common.CustomMessageConstants.User;

namespace KyberKlass.Web.Controllers;
[Authorize(Roles = "Admin")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IUserRoleService _userRoleService;
    private readonly ISchoolService _schoolService;
    private readonly ITeacherService _teacherService;
    private readonly IGuardianService _guardianService;
    private const string CONTROLLER_NAME = "User";

    public UserController(IUserService userService, IUserRoleService userRoleService, ISchoolService schoolService, ITeacherService teacherService, IGuardianService guardianService)
    {
        _userService = userService;
        _userRoleService = userRoleService;
        _schoolService = schoolService;
        _teacherService = teacherService;
        _guardianService = guardianService;
    }

    private static string GetViewPath(string viewName)
    {
        return $"~/Views/Admin/User/{viewName}.cshtml";
    }

    [HttpGet]
    public async Task<IActionResult> All()
    {
        IEnumerable<UserViewModel> allUsersViewModel = await _userService.AllAsync();

        return View(GetViewPath(nameof(All)), allUsersViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Details(string id)
    {
        bool isValidInput = ValidationExtensions.IsNotNullOrEmptyInput<string>(id, null);

        if (isValidInput == false)
        {
            return View("BadRequest400");
            //return BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            UserDetailsViewModel? userDetailsViewModel = await _userService.GetDetailsAsync(id);

            return userDetailsViewModel == null ? View("NotFound404") : (IActionResult)View(GetViewPath(nameof(Details)), userDetailsViewModel);
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

        if (isValidInput == false)
        {
            return View("BadRequest400");
            //return BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            UserEditFormModel? userViewModel = await _userService.GetForEditAsync(id);

            return userViewModel == null ? View("NotFound404") : (IActionResult)View(GetViewPath(nameof(Edit)), userViewModel);
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

        if (isValidInput == false)
        {
            return View("BadRequest400");
            //return BadRequest(INVALID_INPUT_MESSAGE);
        }

        if (ModelState.IsValid == false)
        {
            TempData["ErrorMessage"] = UNABLE_TO_SAVE_CHANGES_MESSAGE;

            return View(GetViewPath(nameof(Edit)), model);
        }

        try
        {
            UserEditFormModel? userToEdit = await _userService.EditAsync(id, model);

            if (userToEdit != null)
            {
                TempData["SuccessMessage"] = model.IsActive == false
                    ? string.Format(SOFT_DELETION_SUCCESSFUL_MESSAGE, CONTROLLER_NAME, model.Id)
                    : (object)string.Format(CHANGES_SUCCESSFULLY_APPLIED_MESSAGE, CONTROLLER_NAME, model.Id);
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

        if (isValidInput == false)
        {
            return View("BadRequest400");
            //return BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            UserUpdateRoleViewModel? userUpdateRoleViewModel = await _userRoleService.GetForUpdateRoleAsync(id);

            return userUpdateRoleViewModel == null
                ? View("NotFound404")
                : (IActionResult)View(GetViewPath(nameof(UpdateRole)), userUpdateRoleViewModel);
        }
        catch (Exception)
        {
            return RedirectToAction(nameof(All));
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRoleConfirmed(string id, string roleId, string? guardianId, string? schoolId, string? classroomId)
    {
        bool isValidInput = ValidationExtensions.IsNotNullOrEmptyInput<string>(id, null) &&
                            ValidationExtensions.IsNotNullOrEmptyInput<string>(roleId, null);

        if (isValidInput == false)
        {
            return View("BadRequest400");
            //return BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            UserUpdateRoleViewModel? userUpdateRoleViewModel = await _userRoleService.GetForUpdateRoleAsync(id); // Retrieve the user viewmodel

            string? currentRole = userUpdateRoleViewModel?.PreviousRoleName; // Gets the name of the role before the update
            string? roleToUpdateTo = await _userRoleService.GetRoleNameByIdAsync(roleId); // Get the name of the role which we want to update to

            if (roleToUpdateTo != null && currentRole == "Teacher") // If user wants to update to a valid role and his current role is "Teacher"
            {
                // Checks if teacher is assigned to a classroom
                bool isTeacherAssignedToClassroom = await _teacherService.IsTeacherAssignedToClassroomAsync(id);

                if (isTeacherAssignedToClassroom) // If teacher is assigned to a classroom
                {
                    // Display a message to change the classroom teacher
                    TempData["ErrorMessage"] = string.Format(FAILED_TO_UPDATE_TEACHER_TO_OTHER_ROLE_MESSAGE, id);
                    return RedirectToAction(nameof(All));
                }
            }
            else if (roleToUpdateTo != null && currentRole == "Guardian") // If user wants to update to a valid role and his current role is "Guardian"
            {
                // Checks if guardian is assigned to any student
                bool isGuardianAssignedToStudent = await _guardianService.IsGuardianAssignedToStudentAsync(id);

                if (isGuardianAssignedToStudent) // If guardian is assigned to any student
                {
                    // Display a message to change the student(s) guardian
                    TempData["ErrorMessage"] = string.Format(FAILED_TO_UPDATE_GUARDIAN_TO_OTHER_ROLE_MESSAGE, id);
                    return RedirectToAction(nameof(All));
                }
            }

            bool successfulRoleUpdate = await _userRoleService.UpdateRoleAsync(id, roleId, guardianId, schoolId, classroomId);

            if (successfulRoleUpdate)
            {
                TempData["SuccessMessage"] = string.Format(ROLE_UPDATE_SUCCESS_MESSAGE, id);

                return RedirectToAction(roleToUpdateTo switch
                {
                    "Teacher" => nameof(All),
                    "Student" or "Guardian" or "Admin" => nameof(All),
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
        IEnumerable<BasicViewModel> guardians = await _guardianService.GetAllGuardiansAsync(); // Fetch guardians
        IEnumerable<BasicViewModel> schools = await _schoolService.BasicAllAsync(); // Fetch schools

        var data = new
        {
            Guardians = guardians,
            Schools = schools
        };

        return Json(data);
    }
}