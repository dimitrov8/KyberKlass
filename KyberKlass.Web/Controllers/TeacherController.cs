namespace KyberKlass.Web.Controllers;

using Microsoft.AspNetCore.Mvc;
using Services.Data.Interfaces;
using ViewModels.Admin.User;

public class TeacherController : Controller
{
	private readonly ITeacherService _teacherService;

	public TeacherController(ITeacherService teacherService)
	{
		this._teacherService = teacherService;
	}

	private string GetViewPath(string viewName)
	{
		return $"~/Views/Admin/Teacher/{viewName}.cshtml";
	}

	public async Task<IActionResult> All()
	{
		List<UserViewModel>? allTeachersViewModel = await this._teacherService.AllAsync();

		return this.View(this.GetViewPath(nameof(this.All)), allTeachersViewModel);
	}
}