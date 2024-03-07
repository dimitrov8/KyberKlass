﻿namespace KyberKlass.Web.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Data.Interfaces;
using ViewModels.Admin.School;
using static Common.CustomMessageConstants.Common;
using static Common.CustomMessageConstants.School;

[Authorize(Roles = "Admin")]
public class SchoolController : Controller
{
	private readonly ISchoolService _schoolService;


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
		bool isValidInput = await this._schoolService.IsNotNullOrEmptyInputAsync(id, null);

		if (isValidInput == false)
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
			return this.RedirectToAction(nameof(this.All), "School"); // Maybe return custom error view
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
				this.TempData["ErrorMessage"] = string.Format(ALREADY_ADDED_MESSAGE, model.Name);
			}
			else
			{
				this.TempData["SuccessMessage"] = string.Format(SUCCESSFULLY_ADDED_MESSAGE, model.Name);
			}

			return this.RedirectToAction(nameof(this.All));
		}
		catch (Exception)
		{
			this.ModelState.AddModelError(string.Empty, ADDITION_ERROR_MESSAGE);

			return this.View(this.GetViewPath(nameof(Add)), model);
		}
	}

	// Gets school by id
	[HttpGet]
	public async Task<IActionResult> Edit(string id)
	{
		bool isValidInput = await this._schoolService.IsNotNullOrEmptyInputAsync(id, null);

		if (isValidInput == false)
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
			return this.RedirectToAction(nameof(this.All), "School");
		}
	}

	[HttpPost]
	public async Task<IActionResult> Edit(string id, SchoolViewModel model)
	{
		bool isValidInput = await this._schoolService.IsNotNullOrEmptyInputAsync(id, model);

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
			bool editSuccessfully = await this._schoolService.EditSchoolAsync(id, model);

			if (editSuccessfully)
			{
				if (model.IsActive == false)
				{
					this.TempData["SuccessDeleteMessage"] = string.Format(SUCCESSFULLY_SOFT_DELETED_MESSAGE, model.Name);
				}
				else
				{
					this.TempData["SuccessMessage"] = string.Format(SUCCESSFULLY_APPLIED_CHANGES, model.Name);
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

	[HttpGet]
	public async Task<IActionResult> Delete(string id)
	{
		bool isValidInput = await this._schoolService.IsNotNullOrEmptyInputAsync(id, null);

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
			return this.RedirectToAction(nameof(this.All), "School");
		}
	}

	[HttpPost]
	public async Task<IActionResult> DeleteConfirmed(string id)
	{
		bool isValidInput = await this._schoolService.IsNotNullOrEmptyInputAsync(id, null);

		if (isValidInput == false)
		{
			return this.BadRequest(INVALID_INPUT_MESSAGE);
		}

		try
		{
			bool successfullyDeleted = await this._schoolService.DeleteAsync(id);

			if (successfullyDeleted)
			{
				this.TempData["SuccessDeleteMessage"] = SUCCESSFULLY_DELETED_MESSAGE;

				return this.RedirectToAction(nameof(this.All));
			}

			return this.BadRequest(DELETION_ERROR_MESSAGE);
		}
		catch (Exception)
		{
			return this.RedirectToAction(nameof(this.All), "School");
		}
	}
}