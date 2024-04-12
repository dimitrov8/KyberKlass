namespace KyberKlass.Web.Controllers;

using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Services.Data.Interfaces;
using ViewModels.Admin;
using ViewModels.Admin.Classroom;
using static Common.CustomMessageConstants.Common;

public class ClassroomController : Controller
{
    private readonly ISchoolService _schoolService;
    private readonly IClassroomService _classroomService;
    private readonly ITeacherService _teacherService;

    private const string CONTROLLER_NAME = "Classroom";

    public ClassroomController(ISchoolService schoolService,
        IClassroomService classroomService,
        ITeacherService teacherService)
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

            var viewModel = new ManageClassroomsViewModel
            {
                SchoolId = schoolId,
                SchoolName = school.Name,
                Classrooms = classrooms.ToList()
            };

            return this.View(this.GetViewPath(nameof(this.Manage)), viewModel);
        }
        catch (Exception)
        {
            return this.NotFound();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Add(string schoolId)
    {
        bool isValidInput = await ValidationExtensions.IsNotNullOrEmptyInputAsync<string>(schoolId, null);

        if (isValidInput == false)
        {
            return this.BadRequest(INVALID_INPUT_MESSAGE);
        }

        IEnumerable<UserBasicViewModel> unassignedTeachers = await this._teacherService.GetUnassignedTeachersAsync();

        var viewModel = new AddClassroomViewModel
        {
            SchoolId = schoolId,
            UnassignedTeachers = unassignedTeachers
        };


        return this.View(this.GetViewPath(nameof(this.Add)), viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddClassroomViewModel model)
    {
        if (this.ModelState.IsValid == false)
        {
            // Repopulate unassigned teachers
            IEnumerable<UserBasicViewModel> unassignedTeachers = await this._teacherService.GetUnassignedTeachersAsync();
            model.UnassignedTeachers = unassignedTeachers;

            // Repopulate unassigned students

            return this.View(this.GetViewPath(nameof(Add)), model);
        }

        try
        {
            bool alreadyExists = await this._classroomService.ClassroomExistsInSchoolAsync(model.Name, model.SchoolId);

            if (alreadyExists)
            {
                this.TempData["ErrorMessage"] = string.Format(ALREADY_ADDED_MESSAGE, CONTROLLER_NAME, model.Name);
            }
            else
            {
                bool addedSuccessfully = await this._classroomService.AddAsync(model);

                if (addedSuccessfully == false)
                {
                    this.TempData["ErrorMessage"] = string.Format(UNABLE_TO_ADD_MESSAGE, CONTROLLER_NAME.ToLower());
                }
                else
                {
                    this.TempData["SuccessMessage"] = string.Format(ADDITION_SUCCESSFUL_MESSAGE, CONTROLLER_NAME, model.Name);
                }
            }

            return this.RedirectToAction(nameof(this.Manage), new { schoolId = model.SchoolId });
        }
        catch (Exception)
        {
            this.ModelState.AddModelError(string.Empty, string.Format(ADDITION_ERROR_MESSAGE, CONTROLLER_NAME.ToLower()));

            // Repopulate unassigned teachers
            IEnumerable<UserBasicViewModel> unassignedTeachers = await this._teacherService.GetUnassignedTeachersAsync();
            model.UnassignedTeachers = unassignedTeachers;

            // Repopulate unassigned students

            return this.View(this.GetViewPath(nameof(Add)), model);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string schoolId, string classroomId)
    {
        bool isValidInput = await ValidationExtensions.IsNotNullOrEmptyInputAsync<string>(schoolId, null) &&
                            await ValidationExtensions.IsNotNullOrEmptyInputAsync<string>(classroomId, null);

        if (isValidInput == false)
        {
            return this.BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            var school = await this._schoolService.GetByIdAsync(schoolId);
            var classroom = await this._classroomService.GetClassroomAsync(classroomId);

            if (school == null && classroom == null)
            {
                // TODO RETURN CUSTOM ERROR PAGE
            }

            //bool isSuccessfullyDeleted = await this._classroomService.DeleteAsync(schoolId, classroomId);
            //if (isSuccessfullyDeleted == false)
            //{
            //	this.TempData["ErrorMessage"] = string.Format(UNABLE_TO_ADD_MESSAGE, CONTROLLER_NAME.ToLower());
            //}
            //else
            //{
            //	this.TempData["SuccessMessage"] = string.Format(DELETION_SUCCESSFUL_MESSAGE, CONTROLLER_NAME, classroom.Id);
            //}
            return this.RedirectToAction(nameof(this.Manage), new { schoolId });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetSchoolClassrooms(string schoolId)
    {
        IEnumerable<UserBasicViewModel> classrooms = await this._classroomService.GetAllClassroomsBySchoolId(schoolId);

        return this.Json(classrooms);
    }
}