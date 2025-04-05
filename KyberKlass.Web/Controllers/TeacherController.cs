using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Web.ViewModels.Admin.User;
using Microsoft.AspNetCore.Mvc;

namespace KyberKlass.Web.Controllers;
public class TeacherController : Controller
{
    private readonly ITeacherService _teacherService;

    public TeacherController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    private string GetViewPath(string viewName)
    {
        return $"~/Views/Admin/Teacher/{viewName}.cshtml";
    }

    [HttpGet]
    public async Task<IActionResult> All()
    {
        List<UserViewModel>? allTeachersViewModel = await _teacherService.AllAsync();

        return View(GetViewPath(nameof(All)), allTeachersViewModel);
    }
}