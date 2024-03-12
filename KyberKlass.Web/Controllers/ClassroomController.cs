namespace KyberKlass.Web.Controllers;

using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Services.Data.Interfaces;
using ViewModels.Admin.Classroom;
using static Common.CustomMessageConstants.Common;

public class ClassroomController : Controller
{
    private readonly ISchoolService _schoolService;
    private readonly IClassroomService _classroomService;
    private readonly ITeacherService _teacherService;

    public ClassroomController(ISchoolService schoolService, IClassroomService classroomService, ITeacherService teacherService)
    {
        this._schoolService = schoolService;
        this._classroomService = classroomService;
        this._teacherService = teacherService;
    }

    private string GetViewPath(string viewName)
    {
        return $"~/Views/Admin/Classroom/{viewName}.cshtml";
    }

    [HttpGet]
    public async Task<IActionResult> Manage(string schoolId)
    {
        bool isValidInput = await ValidationExtensions.IsNotNullOrEmptyInputAsync<string>(schoolId, null);

        if (isValidInput == false)
        {
            return this.BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            var school = await this._schoolService.GetByIdAsync(schoolId);

            if (school == null)
            {
                return this.NotFound();
            }

            IEnumerable<ClassroomViewModel> classrooms = await this._classroomService.GetClassroomsAsync(schoolId);

            ClassroomViewModel[] classroomsViewModel = classrooms
                .Select(c => new ClassroomViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToArray();

            var schoolModel = new ManageClassroomsViewModel
            {
                SchoolId = school.Id,
                SchoolName = school.Name,
                Classrooms = classroomsViewModel
            };

            return this.View(this.GetViewPath(nameof(this.Manage)), schoolModel);
        }
        catch (Exception)
        {
            return this.NotFound();
        }
    }

    //  [HttpGet]
    //  public async Task<IActionResult> Create(string schoolId)
    //  {
    //      bool isValidInput = await ValidationExtensions.IsNotNullOrEmptyInputAsync<string>(schoolId, null);

    //      if (isValidInput == false)
    //      {
    //          return this.BadRequest(INVALID_INPUT_MESSAGE);
    //      }

    //      var unassignedTeachers = await this._teacherService.GetUnassignedTeachersAsync();
    //var unnasignedStudents = await this.
    //  }
}