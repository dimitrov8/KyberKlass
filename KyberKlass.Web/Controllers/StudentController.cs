namespace KyberKlass.Web.Controllers;

using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Data.Interfaces;
using ViewModels.Admin.User;
using static Common.CustomMessageConstants.Common;
using static Common.CustomMessageConstants.Student;

[Authorize(Roles = "Admin")]
public class StudentController : Controller
{
	private readonly IStudentService _studentService;
	private const string CONTROLLER_NAME = "Student";

	public StudentController(IStudentService studentService)
	{
		this._studentService = studentService;
	}

	private string GetViewPath(string viewName)
	{
		return $"~/Views/Admin/Student/{viewName}.cshtml";
	}

	[HttpGet]
	public async Task<IActionResult> All()
	{
		IEnumerable<UserViewModel>? allStudentsViewModel = await this._studentService.AllAsync();

		return this.View(this.GetViewPath(nameof(this.All)), allStudentsViewModel);
	}

	[HttpGet]
	public async Task<IActionResult> ChangeGuardian(string id)
	{
		bool isValidInput = await ValidationExtensions.IsNotNullOrEmptyInputAsync<string>(id, null);

		if (isValidInput == false)
		{
            return this.View("BadRequest400");
            //return this.BadRequest(INVALID_INPUT_MESSAGE);
        }

		try
		{
			var viewModel = await this._studentService.GetStudentChangeGuardianAsync(id);

			if (viewModel.UserDetails == null)
			{
                return this.View("NotFound404");
                //return this.NotFound();
            }

			return this.View(this.GetViewPath(nameof(this.ChangeGuardian)), viewModel);
		}
		catch (Exception)
		{
			return this.RedirectToAction(nameof(this.ChangeGuardian));
		}
	}

	[HttpPost]
	public async Task<IActionResult> ChangeGuardian(string id, string guardianId)
	{
		bool isValidInput = await ValidationExtensions.IsNotNullOrEmptyInputAsync<string>(id, null) &&
		                    await ValidationExtensions.IsNotNullOrEmptyInputAsync<string>(guardianId, null);

		if (isValidInput == false)
		{
            return this.View("BadRequest400");
            //return this.BadRequest(INVALID_INPUT_MESSAGE);
        }

		try
		{
			bool successfulGuardianChange = await this._studentService.StudentChangeGuardianAsync(id, guardianId);

			if (successfulGuardianChange)
			{
				this.TempData["SuccessMessage"] = string.Format(SUCCESSFULLY_CHANGED_GUARDIAN, id);

				return this.RedirectToAction(nameof(this.ChangeGuardian), new { id, guardianId });
			}

			this.TempData["ErrorMessage"] = GUARDIAN_ALREADY_SET;

			return this.RedirectToAction(nameof(this.ChangeGuardian), new { id, guardianId });
		}
		catch (Exception)
		{
			this.TempData["ErrorMessage"] = string.Format(FAILED_TO_CHANGE_GUARDIAN, id);
		}

		return this.RedirectToAction(nameof(ChangeGuardian), new { id, guardianId });
	}
}