#region

using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Web.ViewModels.Admin.User;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace KyberKlass.Web.Controllers;

[Authorize(Roles = "Admin")]
public class TeacherController(ITeacherService teacherService) : Controller
{
    private static string GetViewPath(string viewName)
    {
        return $"~/Views/Admin/Teacher/{viewName}.cshtml";
    }

    [HttpGet]
    public async Task<IActionResult> All(string? searchTerm = null)
    {
        IEnumerable<UserViewModel> teachers = await teacherService.AllAsync(searchTerm);

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView(GetViewPath("_AllPartial"), teachers);
        }

        ViewData["searchTerm"] = searchTerm;
        return View(GetViewPath(nameof(All)), teachers);
    }
}
