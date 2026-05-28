using KyberKlass.Data.Models;
using KyberKlass.Services.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KyberKlass.Web.Controllers;

[Authorize(Roles = "Admin")]
public class TeacherController(
    ITeacherService teacherService,
    IClassroomService classroomService,
    UserManager<ApplicationUser> userManager)
    : Controller
{
    private string GetViewPath(string viewName)
    {
        return $"~/Views/Admin/Teacher/{viewName}.cshtml";
    }

    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        var teacher = await userManager.GetUserAsync(User);

        var classrooms = await classroomService.GetTeacherClassroomsAsync(teacher?.Id.ToString());

        return View(GetViewPath(nameof(Dashboard)), classrooms);
    }

    [HttpGet]
    public async Task<IActionResult> ClassroomStudents(string id)
    {
        var students = await classroomService.GetClassroomStudentsAsync(id);

        return View(GetViewPath(nameof(ClassroomStudents)), students);
    }


    [HttpGet]
    public async Task<IActionResult> All(string? searchTerm = null)
    {
        var teachers = await teacherService.AllAsync(searchTerm);

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView(GetViewPath("_AllPartial"), teachers);
        }

        ViewData["searchTerm"] = searchTerm;
        return View(GetViewPath(nameof(All)), teachers);
    }
}