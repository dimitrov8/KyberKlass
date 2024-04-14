namespace KyberKlass.Web.Controllers;

using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Data.Interfaces;
using ViewModels.Admin;
using ViewModels.Admin.School;
using static Common.CustomMessageConstants.Common;

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
		this._schoolService = schoolService;
	}

	private string GetViewPath(string viewName)
	{
		return $"~/Views/Admin/School/{viewName}.cshtml";
	}

	/// <summary>
	///     Retrieves all schools and renders the corresponding view.
	/// </summary>
	[HttpGet]
	public async Task<IActionResult> All()
	{
		IEnumerable<SchoolDetailsViewModel> schools = await this._schoolService.AllAsync();

		return this.View(this.GetViewPath(nameof(this.All)), schools);
	}

	/// <summary>
	///     Retrieves all schools and returns them as JSON.
	/// </summary>
	[HttpGet]
	public async Task<IActionResult> GetSchools()
	{
		IEnumerable<BasicViewModel> allSchools = await this._schoolService.BasicAllAsync();

		return this.Json(allSchools);
	}

	/// <summary>
	///     Returns a view to add a new school.
	/// </summary>
	[HttpGet]
	public IActionResult Add()
	{
		return this.View(this.GetViewPath(nameof(Add)));
	}

	/// <summary>
	///     Adds a new school.
	/// </summary>
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

			if (addedSuccessfully == false)
			{
				this.TempData["ErrorMessage"] = string.Format(ALREADY_ADDED_MESSAGE, CONTROLLER_NAME, model.Name);
			}
			else
			{
				this.TempData["SuccessMessage"] = string.Format(ADDITION_SUCCESSFUL_MESSAGE, CONTROLLER_NAME, model.Name);
			}

			return this.RedirectToAction(nameof(this.All));
		}
		catch (Exception)
		{
			this.ModelState.AddModelError(string.Empty, ADDITION_ERROR_MESSAGE);

			return this.View(this.GetViewPath(nameof(Add)), model);
		}
	}

	/// <summary>
	///     Displays details of a specific school.
	/// </summary>
	/// <param name="id">The unique identifier of the school.</param>
	[HttpGet]
	public async Task<IActionResult> Details(string id)
	{
		bool isValidInput = await ValidationExtensions.IsNotNullOrEmptyInputAsync<string>(id, null);

		if (isValidInput == false)
		{
			return this.BadRequest(INVALID_INPUT_MESSAGE);
		}

		try
		{
			var schoolModel = await this._schoolService.ViewDetailsAsync(id);

			if (schoolModel == null)
			{
				return this.NotFound();
			}

			return this.View(this.GetViewPath(nameof(this.Details)), schoolModel);
		}
		catch (Exception)
		{
			return this.RedirectToAction(nameof(this.All)); // Maybe return custom error view
		}
	}

	/// <summary>
	///     Displays a view to edit details of a school.
	///     <param name="id">The unique identifier of the school.</param>
	/// </summary>
	[HttpGet]
	public async Task<IActionResult> Edit(string id)
	{
		bool isValidInput = await ValidationExtensions.IsNotNullOrEmptyInputAsync<string>(id, null);

		if (isValidInput == false)
		{
			return this.BadRequest(INVALID_INPUT_MESSAGE);
		}

		try
		{
			var schoolViewModel = await this._schoolService.GetForEditAsync(id);

			if (schoolViewModel == null)
			{
				return this.NotFound();
			}

			return this.View(this.GetViewPath(nameof(this.Edit)), schoolViewModel);
		}
		catch (Exception)
		{
			return this.RedirectToAction(nameof(this.All));
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
		bool isValidInput = await ValidationExtensions.IsNotNullOrEmptyInputAsync<string>(id, null);

		if (isValidInput == false)
		{
			return this.BadRequest(INVALID_INPUT_MESSAGE);
		}

		if (this.ModelState.IsValid == false)
		{
			this.TempData["ErrorMessage"] = UNABLE_TO_SAVE_CHANGES_MESSAGE;

			return this.View(this.GetViewPath(nameof(Edit)), model);
		}

		try
		{
			bool editSuccessfully = await this._schoolService.EditAsync(id, model);

			if (editSuccessfully)
			{
				if (model.IsActive == false)
				{
					this.TempData["SuccessMessage"] = string.Format(SOFT_DELETION_SUCCESSFUL_MESSAGE, CONTROLLER_NAME, model.Id);
				}
				else
				{
					this.TempData["SuccessMessage"] = string.Format(CHANGES_SUCCESSFULLY_APPLIED_MESSAGE, CONTROLLER_NAME, model.Name);
				}
			}

			return this.RedirectToAction(nameof(this.All));
		}
		catch (Exception)
		{
			this.ModelState.AddModelError(string.Empty, EDIT_ERROR_MESSAGE);

			return this.View(this.GetViewPath(nameof(Edit)), model);
		}
	}

	/// <summary>
	///     Displays a confirmation page before deleting a school.
	/// </summary>
	/// <param name="id">The unique identifier of the school to delete.</param>
	[HttpGet]
	public async Task<IActionResult> Delete(string id)
	{
		bool isValidInput = await ValidationExtensions.IsNotNullOrEmptyInputAsync<string>(id, null);

		if (isValidInput == false)
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
			return this.RedirectToAction(nameof(this.All));
		}
	}

	/// <summary>
	///     Deletes the specified school.
	/// </summary>
	/// <param name="id">The unique identifier of the school to delete.</param>
	[HttpPost]
	public async Task<IActionResult> DeleteConfirmed(string id)
	{
		bool isValidInput = await ValidationExtensions.IsNotNullOrEmptyInputAsync<string>(id, null);

		if (isValidInput == false)
		{
			return this.BadRequest(INVALID_INPUT_MESSAGE);
		}

		try
		{
			bool hasStudentsAssigned = await this._schoolService.HasStudentsAssignedAsync(id);

			if (hasStudentsAssigned)
			{
				this.TempData["ErrorMessage"] = string.Format(DELETION_DATA_ERROR_MESSAGE, CONTROLLER_NAME.ToLower());

				return this.RedirectToAction(nameof(this.All), new { id });
			}

			bool successfullyDeleted = await this._schoolService.DeleteAsync(id);

			if (successfullyDeleted)
			{
				this.TempData["SuccessMessage"] = string.Format(DELETION_SUCCESSFUL_MESSAGE, CONTROLLER_NAME, id);

				return this.RedirectToAction(nameof(this.All));
			}

			return this.BadRequest(string.Format(DELETION_ERROR_MESSAGE, CONTROLLER_NAME, id));
		}
		catch (Exception)
		{
			this.TempData["ErrorMessage"] = string.Format(DELETION_ERROR_MESSAGE, CONTROLLER_NAME, id);
			return this.RedirectToAction(nameof(this.All));
		}
	}
}