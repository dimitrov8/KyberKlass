namespace KyberKlass.Web.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Data.Interfaces;
using ViewModels.Admin.User;

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

    public async Task<IActionResult> All()
    {
        List<UserViewModel> users = await this._userService.AllAsync();

        return this.View(this.GetViewPath(nameof(this.All)), users);
    }

    public async Task<IActionResult> Details(string id)
    {
	    var userDetails = await this._userService.GetUserDetailsAsync(id);

	    if (userDetails == null)
	    {
		    return this.NotFound();
	    }

	    return this.View(this.GetViewPath(nameof(this.Details)), userDetails);
    }
}