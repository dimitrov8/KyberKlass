﻿namespace KyberKlass.Web.Controllers;

using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Data.Interfaces;
using ViewModels.Admin;
using ViewModels.Admin.User;
using static Common.CustomMessageConstants.Common;
using static Common.CustomMessageConstants.User;

[Authorize(Roles = "Admin")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly ISchoolService _schoolService;
    private readonly ITeacherService _teacherService;
    private readonly IGuardianService _guardianService;
    private const string CONTROLLER_NAME = "User";

    public UserController(IUserService userService, ISchoolService schoolService, ITeacherService teacherService, IGuardianService guardianService)
    {
        this._userService = userService;
        this._schoolService = schoolService;
        this._teacherService = teacherService;
        this._guardianService = guardianService;
    }

    private string GetViewPath(string viewName)
    {
        return $"~/Views/Admin/User/{viewName}.cshtml";
    }

    [HttpGet]
    public async Task<IActionResult> All()
    {
        IEnumerable<UserViewModel> allUsersViewModel = await this._userService.AllAsync();

        return this.View(this.GetViewPath(nameof(this.All)), allUsersViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Details(string id)
    {
        bool isValidInput = await ValidationExtensions.IsNotNullOrEmptyInputAsync<string>(id, null);

        if (isValidInput == false)
        {
            return this.View("BadRequest400");
            //return this.BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            var userDetailsViewModel = await this._userService.GetDetailsAsync(id);

            if (userDetailsViewModel == null)
            {
                return this.View("NotFound404");
                //return this.NotFound();
            }

            return this.View(this.GetViewPath(nameof(this.Details)), userDetailsViewModel);
        }
        catch (Exception)
        {
            return this.RedirectToAction(nameof(this.All));
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        bool isValidInput = await ValidationExtensions.IsNotNullOrEmptyInputAsync<string>(id, null);

        if (isValidInput == false)
        {
            return this.View("BadRequest400");
            //return this.BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            var userViewModel = await this._userService.GetForEditAsync(id);

            if (userViewModel == null)
            {
                return this.View("NotFound404");
                //return this.NotFound();
            }

            return this.View(this.GetViewPath(nameof(this.Edit)), userViewModel);
        }
        catch (Exception)
        {
            return this.RedirectToAction(nameof(this.All));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(string id, UserEditFormModel model)
    {
        bool isValidInput = await ValidationExtensions.IsNotNullOrEmptyInputAsync<string>(id, null);

        if (isValidInput == false)
        {
            return this.View("BadRequest400");
            //return this.BadRequest(INVALID_INPUT_MESSAGE);
        }

        if (this.ModelState.IsValid == false)
        {
            this.TempData["ErrorMessage"] = UNABLE_TO_SAVE_CHANGES_MESSAGE;

            return this.View(this.GetViewPath(nameof(Edit)), model);
        }

        try
        {
            var userToEdit = await this._userService.EditAsync(id, model);

            if (userToEdit != null)
            {
                if (model.IsActive == false)
                {
                    this.TempData["SuccessMessage"] = string.Format(SOFT_DELETION_SUCCESSFUL_MESSAGE, CONTROLLER_NAME, model.Id);
                }
                else
                {
                    this.TempData["SuccessMessage"] = string.Format(CHANGES_SUCCESSFULLY_APPLIED_MESSAGE, CONTROLLER_NAME, model.Id);
                }
            }

            return this.RedirectToAction(nameof(this.All));
        }
        catch (Exception)
        {
            this.ModelState.AddModelError(string.Empty, string.Format(EDIT_ERROR_MESSAGE, CONTROLLER_NAME.ToLower()));

            return this.View(this.GetViewPath(nameof(Edit)), model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> UpdateRole(string id)
    {
        bool isValidInput = await ValidationExtensions.IsNotNullOrEmptyInputAsync<string>(id, null);

        if (isValidInput == false)
        {
            return this.View("BadRequest400");
            //return this.BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            var userUpdateRoleViewModel = await this._userService.GetForUpdateRoleAsync(id);

            if (userUpdateRoleViewModel == null)
            {
                return this.View("NotFound404");
                //return this.NotFound();
            }

            return this.View(this.GetViewPath(nameof(this.UpdateRole)), userUpdateRoleViewModel);
        }
        catch (Exception)
        {
            return this.RedirectToAction(nameof(this.All));
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRoleConfirmed(string id, string roleId, string? guardianId, string? schoolId, string? classroomId)
    {
        bool isValidInput = await ValidationExtensions.IsNotNullOrEmptyInputAsync<string>(id, null) &&
                            await ValidationExtensions.IsNotNullOrEmptyInputAsync<string>(roleId, null);

        if (isValidInput == false)
        {
            return this.View("BadRequest400");
            //return this.BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            var userUpdateRoleViewModel = await this._userService.GetForUpdateRoleAsync(id); // Retrieve the user viewmodel

            string? currentRole = userUpdateRoleViewModel?.PreviousRoleName; // Gets the name of the role before the update
            string? roleToUpdateTo = await this._userService.GetRoleNameByIdAsync(roleId); // Get the name of the role which we want to update to

            if (roleToUpdateTo != null && currentRole == "Teacher") // If user wants to update to a valid role and his current role is "Teacher"
            {
                // Checks if teacher is assigned to a classroom
                bool isTeacherAssignedToClassroom = await this._teacherService.IsTeacherAssignedToClassroomAsync(id);

                if (isTeacherAssignedToClassroom) // If teacher is assigned to a classroom
                {
                    // Display a message to change the classroom teacher
                    this.TempData["ErrorMessage"] = string.Format(FAILED_TO_UPDATE_TEACHER_TO_OTHER_ROLE_MESSAGE, id);
                    return this.RedirectToAction(nameof(this.All));
                }
            }
            else if (roleToUpdateTo != null && currentRole == "Guardian") // If user wants to update to a valid role and his current role is "Guardian"
            {
                // Checks if guardian is assigned to any student
                bool isGuardianAssignedToStudent = await this._guardianService.IsGuardianAssignedToStudentAsync(id);

                if (isGuardianAssignedToStudent) // If guardian is assigned to any student
                {
                    // Display a message to change the student(s) guardian
                    this.TempData["ErrorMessage"] = string.Format(FAILED_TO_UPDATE_GUARDIAN_TO_OTHER_ROLE_MESSAGE, id);
                    return this.RedirectToAction(nameof(this.All));
                }
            }

            bool successfulRoleUpdate = await this._userService.UpdateRoleAsync(id, roleId, guardianId, schoolId, classroomId);

            if (successfulRoleUpdate)
            {
                this.TempData["SuccessMessage"] = string.Format(ROLE_UPDATE_SUCCESS_MESSAGE, id);

                return this.RedirectToAction(roleToUpdateTo switch
                {
                    "Teacher" => nameof(this.All),
                    "Student" or "Guardian" or "Admin" => nameof(this.All),
                    _ => nameof(this.All)
                });
            }

            return this.View("BadRequest400");
            //return this.BadRequest(string.Format(ROLE_UPDATE_FAILED_MESSAGE, id));
        }
        catch (Exception)
        {
            return this.RedirectToAction(nameof(this.All)); // Can return custom error view
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetGuardiansAndSchools()
    {
        IEnumerable<BasicViewModel> guardians = await this._guardianService.GetAllGuardiansAsync(); // Fetch guardians
        IEnumerable<BasicViewModel> schools = await this._schoolService.BasicAllAsync(); // Fetch schools

        var data = new
        {
            Guardians = guardians,
            Schools = schools
        };

        return this.Json(data);
    }
}