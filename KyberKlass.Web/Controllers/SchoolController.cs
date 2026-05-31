#region

using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Web.ViewModels.Admin;
using KyberKlass.Web.ViewModels.Admin.School;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static KyberKlass.Common.CustomMessageConstants.Common;

#endregion

namespace KyberKlass.Web.Controllers;

[Authorize(Roles = "Admin")]
public class SchoolController(ISchoolService schoolService) : Controller
{
    private const string ControllerName = "School";

    private static string GetViewPath(string viewName)
    {
        return $"~/Views/Admin/{ControllerName}/{viewName}.cshtml";
    }

    [HttpGet]
    public async Task<IActionResult> All(string? searchTerm)
    {
        IEnumerable<SchoolDetailsViewModel> schools = await schoolService.AllAsync(searchTerm);

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView(GetViewPath("_AllPartial"), schools);
        }

        ViewData["searchTerm"] = searchTerm;
        return View(GetViewPath(nameof(All)), schools);
    }

    [HttpGet]
    public async Task<IActionResult> GetSchools()
    {
        IEnumerable<BasicViewModel> allSchools = await schoolService.BasicAllAsync();

        return Json(allSchools);
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View(GetViewPath(nameof(Add)));
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddSchoolFormModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(GetViewPath(nameof(Add)), model);
        }

        try
        {
            bool addedSuccessfully = await schoolService.AddAsync(model);

            if (!addedSuccessfully)
            {
                TempData["ErrorMessage"] = string.Format(ALREADY_ADDED_MESSAGE, ControllerName, model.Name);
            }
            else
            {
                TempData["SuccessMessage"] = string.Format(ADDITION_SUCCESSFUL_MESSAGE, ControllerName, model.Name);
            }

            return RedirectToAction(nameof(All));
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, ADDITION_ERROR_MESSAGE);

            return View(GetViewPath(nameof(Add)), model);
        }
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
            SchoolDetailsViewModel? schoolModel = await schoolService.ViewDetailsAsync(id);

            return schoolModel == null ? View("NotFound404") : View(GetViewPath(nameof(Details)), schoolModel);
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
            AddSchoolFormModel? schoolViewModel = await schoolService.GetForEditAsync(id);

            return schoolViewModel == null ? View("NotFound404") : View(GetViewPath(nameof(Edit)), schoolViewModel);
        }
        catch (Exception)
        {
            return RedirectToAction(nameof(All));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(string id, AddSchoolFormModel model)
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
            bool editSuccessfully = await schoolService.EditAsync(id, model);

            if (editSuccessfully)
            {
                TempData["SuccessMessage"] = !model.IsActive
                    ? string.Format(SOFT_DELETION_SUCCESSFUL_MESSAGE, ControllerName, model.Id)
                    : string.Format(CHANGES_SUCCESSFULLY_APPLIED_MESSAGE, ControllerName, model.Name);
            }

            return RedirectToAction(nameof(All));
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, EDIT_ERROR_MESSAGE);

            return View(GetViewPath(nameof(Edit)), model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return View("BadRequest400");
        }

        try
        {
            SchoolDetailsViewModel? model = await schoolService.GetForDeleteAsync(id);

            return model == null ? View("NotFound404") : View(GetViewPath(nameof(Delete)), model);
        }
        catch (Exception)
        {
            return RedirectToAction(nameof(All));
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return View("BadRequest400");
        }

        try
        {
            bool hasStudentsAssigned = await schoolService.HasStudentsAssignedAsync(id);

            if (hasStudentsAssigned)
            {
                TempData["ErrorMessage"] = string.Format(DELETION_DATA_ERROR_MESSAGE, ControllerName.ToLower());

                return RedirectToAction(nameof(All), new { id });
            }

            bool successfullyDeleted = await schoolService.DeleteAsync(id);

            if (successfullyDeleted)
            {
                TempData["SuccessMessage"] = string.Format(DELETION_SUCCESSFUL_MESSAGE, ControllerName, id);

                return RedirectToAction(nameof(All));
            }

            return BadRequest(string.Format(DELETION_ERROR_MESSAGE, ControllerName, id));
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = string.Format(DELETION_ERROR_MESSAGE, ControllerName, id);
            return RedirectToAction(nameof(All));
        }
    }
}
