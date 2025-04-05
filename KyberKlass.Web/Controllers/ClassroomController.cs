using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Web.Infrastructure.Extensions;
using KyberKlass.Web.ViewModels.Admin;
using KyberKlass.Web.ViewModels.Admin.Classroom;
using Microsoft.AspNetCore.Mvc;
using static KyberKlass.Common.CustomMessageConstants.Common;

namespace KyberKlass.Web.Controllers;
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
        _schoolService = schoolService;
        _classroomService = classroomService;
        _teacherService = teacherService;
    }

    private string GetViewPath(string viewName)
    {
        return $"~/Views/Admin/Classroom/{viewName}.cshtml";
    }

    [HttpGet]
    public async Task<IActionResult> Add(string schoolId)
    {
        bool isValidInput = ValidationExtensions.IsNotNullOrEmptyInput<string>(schoolId, null);

        if (isValidInput == false)
        {
            return View("BadRequest400");
            //return BadRequest(INVALID_INPUT_MESSAGE);
        }

        IEnumerable<BasicViewModel> unassignedTeachers = await _teacherService.GetUnassignedTeachersAsync();

        AddClassroomViewModel viewModel = new()
        {
            SchoolId = schoolId,
            UnassignedTeachers = unassignedTeachers
        };


        return View(GetViewPath(nameof(Add)), viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddClassroomViewModel model)
    {
        if (ModelState.IsValid == false)
        {
            // Repopulate unassigned teachers
            IEnumerable<BasicViewModel> unassignedTeachers = await _teacherService.GetUnassignedTeachersAsync();
            model.UnassignedTeachers = unassignedTeachers;

            // Repopulate unassigned students

            return View(GetViewPath(nameof(Add)), model);
        }

        try
        {
            bool alreadyExists = await _classroomService.ClassroomExistsInSchoolAsync(model.Name, model.SchoolId);

            if (alreadyExists)
            {
                TempData["ErrorMessage"] = string.Format(ALREADY_ADDED_MESSAGE, CONTROLLER_NAME, model.Name);
            }
            else
            {
                bool addedSuccessfully = await _classroomService.AddAsync(model);

                if (addedSuccessfully == false)
                {
                    TempData["ErrorMessage"] = string.Format(UNABLE_TO_ADD_MESSAGE, CONTROLLER_NAME.ToLower());
                }
                else
                {
                    TempData["SuccessMessage"] = string.Format(ADDITION_SUCCESSFUL_MESSAGE, CONTROLLER_NAME, model.Name);
                }
            }

            return RedirectToAction(nameof(Manage), new { schoolId = model.SchoolId });
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, string.Format(ADDITION_ERROR_MESSAGE, CONTROLLER_NAME.ToLower()));

            // Repopulate unassigned teachers
            IEnumerable<BasicViewModel> unassignedTeachers = await _teacherService.GetUnassignedTeachersAsync();
            model.UnassignedTeachers = unassignedTeachers;

            // Repopulate unassigned students

            return View(GetViewPath(nameof(Add)), model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetSchoolClassrooms(string schoolId)
    {
        IEnumerable<BasicViewModel> classrooms = await _classroomService.GetAllClassroomsBySchoolIdAsJsonAsync(schoolId);

        return Json(classrooms);
    }

    [HttpGet]
    public async Task<IActionResult> Details(string schoolId, string classroomId)
    {
        bool isValidInput = ValidationExtensions.IsNotNullOrEmptyInput<string>(schoolId, null) &&
                            ValidationExtensions.IsNotNullOrEmptyInput<string>(classroomId, null);

        if (isValidInput == false)
        {
            return View("BadRequest400");
            //return BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            ClassroomDetailsViewModel? classroomDetailsViewModel = await _classroomService.GetClassroomAsync(classroomId);

            return classroomDetailsViewModel == null
                ? View("NotFound404")
                : (IActionResult)View(GetViewPath(nameof(Details)), classroomDetailsViewModel);
        }
        catch (Exception)
        {
            return RedirectToAction(nameof(Manage), new { schoolId });
        }
    }

    [HttpGet]
    public async Task<IActionResult> Manage(string schoolId)
    {
        bool isValidInput = ValidationExtensions.IsNotNullOrEmptyInput<string>(schoolId, null);

        if (isValidInput == false)
        {
            return View("BadRequest400");
            //return BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            ViewModels.Admin.School.SchoolDetailsViewModel? school = await _schoolService.GetByIdAsync(schoolId);

            if (school == null)
            {
                return View("NotFound404");
                //return NotFound();
            }

            IEnumerable<ClassroomDetailsViewModel> classrooms = await _classroomService.AllAsync(schoolId);

            ManageClassroomsViewModel viewModel = new()
            {
                SchoolId = schoolId,
                SchoolName = school.Name,
                Classrooms = classrooms.ToList()
            };

            return View(GetViewPath(nameof(Manage)), viewModel);
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string schoolId, string classroomId)
    {
        bool isValidInput = ValidationExtensions.IsNotNullOrEmptyInput<string>(schoolId, null) &&
                            ValidationExtensions.IsNotNullOrEmptyInput<string>(classroomId, null);

        if (isValidInput == false)
        {
            return View("BadRequest400");
            //return BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            AddClassroomViewModel? classroomViewModel = await _classroomService.GetForEditAsync(classroomId);

            return classroomViewModel == null ? View("NotFound404") : (IActionResult)View(GetViewPath(nameof(Edit)), classroomViewModel);
        }
        catch (Exception)
        {
            return RedirectToAction(nameof(Manage), new { schoolId });
        }
    }

    /// <summary>
    ///     Updates the details of a school.
    /// </summary>
    /// <param name="classroomId">The unique identifier of the classroom.</param>
    /// <param name="model">The <see cref="ClassroomDetailsViewModel" /> containing the updated details.</param>
    [HttpPost]
    public async Task<IActionResult> Edit(string classroomId, AddClassroomViewModel model)
    {
        bool isValidInput = ValidationExtensions.IsNotNullOrEmptyInput<string>(classroomId, null);

        if (isValidInput == false)
        {
            return View("BadRequest400");
            //return BadRequest(INVALID_INPUT_MESSAGE);
        }

        if (ModelState.IsValid == false)
        {
            TempData["ErrorMessage"] = UNABLE_TO_SAVE_CHANGES_MESSAGE;

            return View(GetViewPath(nameof(Edit)), model);
        }

        try
        {
            bool editSuccessfully = await _classroomService.EditAsync(classroomId, model);

            if (editSuccessfully)
            {
                TempData["SuccessMessage"] = model.IsActive == false
                    ? string.Format(SOFT_DELETION_SUCCESSFUL_MESSAGE, CONTROLLER_NAME, model.Id)
                    : (object)string.Format(CHANGES_SUCCESSFULLY_APPLIED_MESSAGE, CONTROLLER_NAME, model.Name);
            }

            return RedirectToAction(nameof(Manage), new { model.SchoolId });
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, EDIT_ERROR_MESSAGE);

            return View(GetViewPath(nameof(Edit)), model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(string schoolId, string classroomId)
    {
        bool isValidInput = ValidationExtensions.IsNotNullOrEmptyInput<string>(schoolId, null) &&
                            ValidationExtensions.IsNotNullOrEmptyInput<string>(classroomId, null);

        if (isValidInput == false)
        {
            return View("BadRequest400");
            //return BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            ClassroomDetailsViewModel? model = await _classroomService.GetForDeleteAsync(classroomId);

            return model == null ? View("NotFound404") : (IActionResult)View(GetViewPath(nameof(Delete)), model);
        }
        catch (Exception)
        {
            return RedirectToAction(nameof(Manage), new { schoolId });
        }
    }

    /// <summary>
    ///     Displays a confirmation page before deleting a classroom.
    /// </summary>
    /// <param name="schoolId">The unique identifier of the school the classroom is in.</param>
    /// <param name="classroomId">The unique identifier of the classroom.</param>
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(string schoolId, string classroomId)
    {
        bool isValidInput = ValidationExtensions.IsNotNullOrEmptyInput<string>(schoolId, null) &&
                            ValidationExtensions.IsNotNullOrEmptyInput<string>(classroomId, null);

        if (isValidInput == false)
        {
            return View("BadRequest400");
            //return BadRequest(INVALID_INPUT_MESSAGE);
        }

        try
        {
            bool hasStudentsAssigned = await _classroomService.HasStudentsAssignedAsync(classroomId);

            if (hasStudentsAssigned)
            {
                TempData["ErrorMessage"] = string.Format(DELETION_DATA_ERROR_MESSAGE, CONTROLLER_NAME.ToLower());

                return RedirectToAction(nameof(Manage), new { schoolId });
            }

            bool successfullyDeleted = await _classroomService.DeleteAsync(classroomId);

            if (successfullyDeleted)
            {
                TempData["SuccessMessage"] = string.Format(DELETION_SUCCESSFUL_MESSAGE, CONTROLLER_NAME, classroomId);

                return RedirectToAction(nameof(Manage), new { schoolId });
            }

            return BadRequest(string.Format(DELETION_ERROR_MESSAGE, CONTROLLER_NAME, classroomId));
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = string.Format(DELETION_ERROR_MESSAGE, CONTROLLER_NAME, classroomId);
            return RedirectToAction(nameof(Manage), new { schoolId });
        }
    }
}