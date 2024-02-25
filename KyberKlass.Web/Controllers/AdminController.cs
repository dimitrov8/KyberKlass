namespace KyberKlass.Web.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "admin")]
public class AdminController : Controller
{
	public IActionResult Dashboard()
	{
		return this.View();
	}
}