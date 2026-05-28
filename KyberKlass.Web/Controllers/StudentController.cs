using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Web.Infrastructure.Extensions;
using KyberKlass.Web.ViewModels.Admin.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static KyberKlass.Common.CustomMessageConstants.Student;

namespace KyberKlass.Web.Controllers;

[Authorize(Roles = "Admin")]
public class StudentController(IStudentService studentService) : Controller
{
    private const string CONTROLLER_NAME = "Student";

    private string GetViewPath(string viewName)
    {
        return $"~/Views/Admin/Student/{viewName}.cshtml";
    }

    [HttpGet]
    public async Task<IActionResult> All(string? searchTerm)
    {
        IEnumerable<UserViewModel>? students = await studentService.AllAsync(searchTerm);

        if (Request.Headers.XRequestedWith == "XMLHttpRequest")
        {
            return PartialView(GetViewPath("_AllPartial"), students);
        }

        ViewData["searchTerm"] = searchTerm;
        return View(GetViewPath(nameof(All)), students);
    }

    [HttpGet]
    public async Task<IActionResult> ChangeGuardian(string id)
    {
        bool isValidInput = ValidationExtensions.IsNotNullOrEmptyInput<string>(id, null);

        if (isValidInput == false)
        {
            return View("BadRequest400");
            //return BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            ViewModels.Admin.Student.StudentChangeGuardianViewModel viewModel = await studentService.GetStudentChangeGuardianAsync(id);

            return viewModel.UserDetails == null ? View("NotFound404") : View(GetViewPath(nameof(ChangeGuardian)), viewModel);
        }
        catch (Exception)
        {
            return RedirectToAction(nameof(ChangeGuardian));
        }
    }

    [HttpPost]
    public async Task<IActionResult> ChangeGuardian(string id, string guardianId)
    {
        bool isValidInput = ValidationExtensions.IsNotNullOrEmptyInput<string>(id, null) &&
                            ValidationExtensions.IsNotNullOrEmptyInput<string>(guardianId, null);

        if (isValidInput == false)
        {
            return View("BadRequest400");
            //return BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            bool successfulGuardianChange = await studentService.StudentChangeGuardianAsync(id, guardianId);

            if (successfulGuardianChange)
            {
                TempData["SuccessMessage"] = string.Format(SUCCESSFULLY_CHANGED_GUARDIAN, id);

                return RedirectToAction(nameof(ChangeGuardian), new { id, guardianId });
            }

            TempData["ErrorMessage"] = GUARDIAN_ALREADY_SET;

            return RedirectToAction(nameof(ChangeGuardian), new { id, guardianId });
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = string.Format(FAILED_TO_CHANGE_GUARDIAN, id);
        }

        return RedirectToAction(nameof(ChangeGuardian), new { id, guardianId });
    }
}