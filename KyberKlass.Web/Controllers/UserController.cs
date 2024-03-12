namespace KyberKlass.Web.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Data.Interfaces;
using ViewModels.Admin.User;
using static Common.CustomMessageConstants.Common;
using static Common.CustomMessageConstants.User;

[Authorize(Roles = "Admin")]
public class UserController : Controller
{
	private readonly IUserService _userService;

	public UserController(IUserService userService)
	{
		this._userService = userService;
	}

	private string GetViewPath(string viewName)
	{
		return $"~/Views/Admin/User/{viewName}.cshtml";
	}

	[HttpGet]
	public async Task<IActionResult> All()
	{
		List<UserViewModel> allUsersViewModel = await this._userService.AllAsync();

		return this.View(this.GetViewPath(nameof(this.All)), allUsersViewModel);
	}

	[HttpGet]
	public async Task<IActionResult> Details(string id)
	{
		bool isValidInput = await this._userService.IsNotNullOrEmptyInputAsync(id, null);

		if (isValidInput == false)
		{
			return this.BadRequest(INVALID_INPUT_MESSAGE);
		}

		try
		{
			var userDetailsViewModel = await this._userService.GetUserDetailsAsync(id);

			if (userDetailsViewModel == null)
			{
				return this.NotFound();
			}

			return this.View(this.GetViewPath(nameof(this.Details)), userDetailsViewModel);
		}
		catch (Exception)
		{
			return this.RedirectToAction(nameof(this.All), "User");
		}
	}

	[HttpGet]
	public async Task<IActionResult> UpdateRole(string id)
	{
		bool isValidInput = await this._userService.IsNotNullOrEmptyInputAsync(id, null);

		if (isValidInput == false)
		{
			return this.BadRequest(INVALID_INPUT_MESSAGE);
		}

		try
		{
			var userUpdateRoleViewModel = await this._userService.GetUserForUpdateRoleAsync(id);

			if (userUpdateRoleViewModel == null)
			{
				return this.NotFound();
			}

			return this.View(this.GetViewPath(nameof(this.UpdateRole)), userUpdateRoleViewModel);
		}
		catch (Exception)
		{
			return this.RedirectToAction(nameof(this.All), "User");
		}
	}

	[HttpPost]
	public async Task<IActionResult> UpdateRoleConfirmed(string id, string roleId)
	{
		bool isValidInput = await this._userService.IsNotNullOrEmptyInputAsync(id, null) &&
		                    await this._userService.IsNotNullOrEmptyInputAsync(roleId, null);

		if (isValidInput == false)
		{
			return this.BadRequest(INVALID_INPUT_MESSAGE);
		}

		try
		{
			string? updatedRoleName = await this._userService.UpdateRoleAsync(id, roleId);

			if (string.IsNullOrEmpty(updatedRoleName) == false)
			{
				this.TempData["RoleUpdateSuccessMessage"] = ROLE_UPDATE_SUCCESS_MESSAGE;

				if (updatedRoleName == "Teacher")
				{
					return this.RedirectToAction(nameof(this.All), "Teacher");
				}

				return this.RedirectToAction(nameof(this.All), "User");
			}

			return this.BadRequest(ROLE_UPDATE_FAILED_MESSAGE);
		}
		catch (Exception)
		{
			return this.RedirectToAction(nameof(this.All), "User"); // Can return custom error view
		}
	}
}