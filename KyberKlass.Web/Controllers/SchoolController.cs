namespace KyberKlass.Web.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Data.Interfaces;
using ViewModels.Admin.School;

[Authorize(Roles = "Admin")]
public class SchoolController : Controller
{
    private readonly ISchoolService _schoolService;

    private const string INVALID_INPUT_MESSAGE = "Invalid input. Please check your data and try again.";

    public SchoolController(ISchoolService schoolService)
    {
        this._schoolService = schoolService;
    }

    private string GetViewPath(string viewName)
    {
        return $"~/Views/Admin/School/{viewName}.cshtml";
    }

    // Shows all schools
    [HttpGet]
    public async Task<IActionResult> All()
    {
        IEnumerable<SchoolViewModel> schools = await this._schoolService.AllAsync();

        return this.View(this.GetViewPath(nameof(this.All)), schools);
    }

    [HttpGet]
    public async Task<IActionResult> Details(string id)
    {
        bool isValid = await this._schoolService.ValidateInputAsync(id, null);

        if (isValid == false)
        {
            return this.BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            var school = await this._schoolService.ViewDetailsAsync(id);

            if (school == null)
            {
                return this.NotFound();
            }

            return this.View(this.GetViewPath(nameof(this.Details)), school);
        }
        catch (Exception)
        {
            return this.StatusCode(500, "An error occurred while processing your request. Please try again later.");
        }
    }

    // Returns a view from which we can add a school
    [HttpGet]
    public IActionResult Add()
    {
        return this.View(this.GetViewPath(nameof(Add)));
    }

    // Tries to add the school
    [HttpPost]
    public async Task<IActionResult> Add(AddSchoolFormModel model)
    {
        if (this.ModelState.IsValid == false)
        {
            return this.View(this.GetViewPath(nameof(Add)), model);
        }

        try
        {
            bool addedSuccessfully = await this._schoolService.AddAsync(model);

            if (!addedSuccessfully)
            {
                this.TempData["ErrorMessage"] = "School already added.";
            }
            else
            {
                this.TempData["SuccessMessage"] = $"Successfully added School \"{model.Name}\".";
            }

            return this.RedirectToAction(nameof(this.All));
        }
        catch (Exception)
        {
            this.ModelState.AddModelError(string.Empty, "An error occurred while adding the school.");

            return this.View(this.GetViewPath(nameof(Add)), model);
        }
    }

    // Gets school by id
    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        bool isValid = await this._schoolService.ValidateInputAsync(id, null);

        if (isValid == false)
        {
            return this.BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            var school = await this._schoolService.GetForEditAsync(id);

            if (school == null)
            {
                return this.NotFound();
            }

            return this.View(this.GetViewPath(nameof(this.Edit)), school);
        }
        catch (Exception)
        {
            return this.StatusCode(500, "An error occurred while processing your request. Please try again later.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(string id, SchoolViewModel model)
    {
        bool isValid = await this._schoolService.ValidateInputAsync(id, model);

        if (isValid == false)
        {
            return this.BadRequest(INVALID_INPUT_MESSAGE);
        }

        if (this.ModelState.IsValid == false)
        {
            this.TempData["ErrorMessage"] = "Something went wrong trying to save the changes you made. Please try again.";

            return this.View(this.GetViewPath(nameof(Edit)), model);
        }

        try
        {
            bool editSuccessfully = await this._schoolService.EditSchoolAsync(id, model);

            if (editSuccessfully)
            {
                if (model.IsDeleted)
                {
                    this.TempData["SuccessDeleteMessage"] = $"Successfully soft deleted School : \"{model.Name}\".";
                }
                else
                {
                    this.TempData["SuccessMessage"] = $"Successfully applied changes for School: \"{model.Name}\". ";
                }
            }

            return this.RedirectToAction(nameof(this.All));
        }
        catch (Exception)
        {
            this.ModelState.AddModelError(string.Empty, "An error occurred while editing the school.");

            return this.View(this.GetViewPath(nameof(Edit)), model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        bool isValid = await this._schoolService.ValidateInputAsync(id, null);

        if (isValid == false)
        {
            return this.BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            var model = await this._schoolService.GetForDeleteAsync(id);

            if (model == null)
            {
                return this.NotFound();
            }

            return this.View(this.GetViewPath(nameof(this.Delete)), model);
        }
        catch (Exception)
        {
            return this.RedirectToAction(nameof(this.All), "School");
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        try
        {
            bool successfullyDeleted = await this._schoolService.DeleteAsync(id);

            if (successfullyDeleted)
            {
                this.TempData["SuccessDeleteMessage"] = $"School successfully deleted.";

                return this.RedirectToAction(nameof(this.All));
            }

            return this.BadRequest("Failed to delete the school. Please try again later.");
        }
        catch (Exception)
        {
            return this.StatusCode(500, "An error occurred while processing your request. Please try again later.");
        }
    }
}