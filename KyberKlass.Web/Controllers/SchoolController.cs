using AspNetCoreGeneratedDocument;
using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Web.Infrastructure.Extensions;
using KyberKlass.Web.ViewModels.Admin;
using KyberKlass.Web.ViewModels.Admin.School;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using static KyberKlass.Common.CustomMessageConstants.Common;

namespace KyberKlass.Web.Controllers;
/// <summary>
///     Controller responsible for managing schools in the system.
/// </summary>
[Authorize(Roles = "Admin")]
public class SchoolController : Controller
{
    private readonly ISchoolService _schoolService;
    private const string CONTROLLER_NAME = "School";

    /// <summary>
    ///     Initializes a new instance of the <see cref="SchoolController" /> class.
    /// </summary>
    /// <param name="schoolService">The service for managing schools.</param>
    public SchoolController(ISchoolService schoolService)
    {
        _schoolService = schoolService;
    }

    private string GetViewPath(string viewName)
    {
        return $"~/Views/Admin/School/{viewName}.cshtml";
    }

    /// <summary>
    ///     Retrieves all schools and renders the corresponding view.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> All(string? searchTerm)
    {
        IEnumerable<SchoolDetailsViewModel> schools = await _schoolService.AllAsync(searchTerm);

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView(GetViewPath("_AllPartial"), schools);
        }

        ViewData["searchTerm"] = searchTerm;
        return View(GetViewPath(nameof(All)), schools);
    }

    /// <summary>
    ///     Retrieves all schools and returns them as JSON.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetSchools()
    {
        IEnumerable<BasicViewModel> allSchools = await _schoolService.BasicAllAsync();

        return Json(allSchools);
    }

    /// <summary>
    ///     Returns a view to add a new school.
    /// </summary>
    [HttpGet]
    public IActionResult Add()
    {
        return View(GetViewPath(nameof(Add)));
    }

    /// <summary>
    ///     Adds a new school.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Add(AddSchoolFormModel model)
    {
        if (ModelState.IsValid == false)
        {
            return View(GetViewPath(nameof(Add)), model);
        }

        try
        {
            bool addedSuccessfully = await _schoolService.AddAsync(model);

            if (addedSuccessfully == false)
            {
                TempData["ErrorMessage"] = string.Format(ALREADY_ADDED_MESSAGE, CONTROLLER_NAME, model.Name);
            }
            else
            {
                TempData["SuccessMessage"] = string.Format(ADDITION_SUCCESSFUL_MESSAGE, CONTROLLER_NAME, model.Name);
            }

            return RedirectToAction(nameof(All));
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, ADDITION_ERROR_MESSAGE);

            return View(GetViewPath(nameof(Add)), model);
        }
    }

    /// <summary>
    ///     Displays details of a specific school.
    /// </summary>
    /// <param name="id">The unique identifier of the school.</param>
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
            SchoolDetailsViewModel? schoolModel = await _schoolService.ViewDetailsAsync(id);

            return schoolModel == null ? View("NotFound404") : (IActionResult)View(GetViewPath(nameof(Details)), schoolModel);
        }
        catch (Exception)
        {
            return RedirectToAction(nameof(All)); // Maybe return custom error view
        }
    }

    /// <summary>
    ///     Displays a view to edit details of a school.
    ///     <param name="id">The unique identifier of the school.</param>
    /// </summary>
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
            AddSchoolFormModel? schoolViewModel = await _schoolService.GetForEditAsync(id);

            return schoolViewModel == null ? View("NotFound404") : (IActionResult)View(GetViewPath(nameof(Edit)), schoolViewModel);
        }
        catch (Exception)
        {
            return RedirectToAction(nameof(All));
        }
    }

    /// <summary>
    ///     Updates the details of a school.
    /// </summary>
    /// <param name="id">The unique identifier of the school.</param>
    /// <param name="model">The <see cref="SchoolDetailsViewModel" /> containing the updated details.</param>
    [HttpPost]
    public async Task<IActionResult> Edit(string id, AddSchoolFormModel model)
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
            bool editSuccessfully = await _schoolService.EditAsync(id, model);

            if (editSuccessfully)
            {
                TempData["SuccessMessage"] = model.IsActive == false
                    ? string.Format(SOFT_DELETION_SUCCESSFUL_MESSAGE, CONTROLLER_NAME, model.Id)
                    : (object)string.Format(CHANGES_SUCCESSFULLY_APPLIED_MESSAGE, CONTROLLER_NAME, model.Name);
            }

            return RedirectToAction(nameof(All));
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, EDIT_ERROR_MESSAGE);

            return View(GetViewPath(nameof(Edit)), model);
        }
    }

    /// <summary>
    ///     Displays a confirmation page before deleting a school.
    /// </summary>
    /// <param name="id">The unique identifier of the school to delete.</param>
    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        bool isValidInput = ValidationExtensions.IsNotNullOrEmptyInput<string>(id, null);

        if (isValidInput == false)
        {
            return View("BadRequest400");
            //return BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            SchoolDetailsViewModel? model = await _schoolService.GetForDeleteAsync(id);

            return model == null ? View("NotFound404") : (IActionResult)View(GetViewPath(nameof(Delete)), model);
        }
        catch (Exception)
        {
            return RedirectToAction(nameof(All));
        }
    }

    /// <summary>
    ///     Deletes the specified school.
    /// </summary>
    /// <param name="id">The unique identifier of the school to delete.</param>
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        bool isValidInput = ValidationExtensions.IsNotNullOrEmptyInput<string>(id, null);

        if (isValidInput == false)
        {
            return View("BadRequest400");
            //return BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            bool hasStudentsAssigned = await _schoolService.HasStudentsAssignedAsync(id);

            if (hasStudentsAssigned)
            {
                TempData["ErrorMessage"] = string.Format(DELETION_DATA_ERROR_MESSAGE, CONTROLLER_NAME.ToLower());

                return RedirectToAction(nameof(All), new { id });
            }

            bool successfullyDeleted = await _schoolService.DeleteAsync(id);

            if (successfullyDeleted)
            {
                TempData["SuccessMessage"] = string.Format(DELETION_SUCCESSFUL_MESSAGE, CONTROLLER_NAME, id);

                return RedirectToAction(nameof(All));
            }

            return BadRequest(string.Format(DELETION_ERROR_MESSAGE, CONTROLLER_NAME, id));
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = string.Format(DELETION_ERROR_MESSAGE, CONTROLLER_NAME, id);
            return RedirectToAction(nameof(All));
        }
    }
}