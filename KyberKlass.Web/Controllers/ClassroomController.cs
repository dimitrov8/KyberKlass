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
	public async Task<IActionResult> Add(string schoolId)
	{
		bool isValidInput = await ValidationExtensions.IsNotNullOrEmptyInputAsync<string>(schoolId, null);

		if (isValidInput == false)
		{
			return this.BadRequest(INVALID_INPUT_MESSAGE);
		}

		IEnumerable<BasicViewModel> unassignedTeachers = await this._teacherService.GetUnassignedTeachersAsync();

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
			IEnumerable<BasicViewModel> unassignedTeachers = await this._teacherService.GetUnassignedTeachersAsync();
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
			IEnumerable<BasicViewModel> unassignedTeachers = await this._teacherService.GetUnassignedTeachersAsync();
			model.UnassignedTeachers = unassignedTeachers;

			// Repopulate unassigned students

			return this.View(this.GetViewPath(nameof(Add)), model);
		}
	}

	[HttpGet]
	public async Task<IActionResult> GetSchoolClassrooms(string schoolId)
	{
		IEnumerable<BasicViewModel> classrooms = await this._classroomService.GetAllClassroomsBySchoolIdAsJsonAsync(schoolId);

		return this.Json(classrooms);
	}

	[HttpGet]
	public async Task<IActionResult> Details(string schoolId, string classroomId)
	{
		bool isValidInput = await ValidationExtensions.IsNotNullOrEmptyInputAsync<string>(schoolId, null) &&
		                    await ValidationExtensions.IsNotNullOrEmptyInputAsync<string>(classroomId, null);

		if (isValidInput == false)
		{
			return this.BadRequest(INVALID_INPUT_MESSAGE);
		}

		try
		{
			var classroomDetailsViewModel = await this._classroomService.GetClassroomAsync(schoolId, classroomId);

			if (classroomDetailsViewModel == null)
			{
				return this.NotFound(); // If classroom is not found
			}

			return this.View(this.GetViewPath(nameof(this.Details)), classroomDetailsViewModel);
		}
		catch (Exception)
		{
			return this.RedirectToAction(nameof(this.Manage), new { schoolId });
		}
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

			IEnumerable<ClassroomDetailsViewModel> classrooms = await this._classroomService.GetAllClassroomsAsync(schoolId);

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
            var model = await this._classroomService.GetForDeleteAsync(schoolId, classroomId);

            if (model == null)
            {
				return this.NotFound();
			}

            return this.View(this.GetViewPath(nameof(this.Delete)), model);
        }
        catch (Exception)
        {
			return this.RedirectToAction(nameof(this.Manage), new { schoolId });
		}
    }
}