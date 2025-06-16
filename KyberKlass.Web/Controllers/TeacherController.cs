using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Web.ViewModels.Admin.User;
using Microsoft.AspNetCore.Mvc;
using static KyberKlass.Common.CustomMessageConstants;

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
    public async Task<IActionResult> All(string? searchTerm = null)
    {
        IEnumerable<UserViewModel> teachers = await _teacherService.AllAsync(searchTerm);

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView(GetViewPath("_AllPartial"), teachers);
        }

        ViewData["searchTerm"] = searchTerm;
        return View(GetViewPath(nameof(All)), teachers);
    }
}