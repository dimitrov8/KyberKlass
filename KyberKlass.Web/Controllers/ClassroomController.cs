namespace KyberKlass.Web.Controllers;

using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Services.Data.Interfaces;
using ViewModels.Admin.Classroom;
using ViewModels.Admin.User;
using static Common.CustomMessageConstants.Common;

public class ClassroomController : Controller
{
	private readonly ISchoolService _schoolService;
	private readonly IClassroomService _classroomService;
	private readonly ITeacherService _teacherService;
	private readonly IStudentService _studentService;

	private const string CONTROLLER_NAME = "Classroom";

	public ClassroomController(ISchoolService schoolService,
		IClassroomService classroomService,
		ITeacherService teacherService,
		IStudentService studentService)
	{
		this._schoolService = schoolService;
		this._classroomService = classroomService;
		this._teacherService = teacherService;
		this._studentService = studentService;
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

	[HttpGet]
	public async Task<IActionResult> Add(string schoolId)
	{
		bool isValidInput = await ValidationExtensions.IsNotNullOrEmptyInputAsync<string>(schoolId, null);

		if (isValidInput == false)
		{
			return this.BadRequest(INVALID_INPUT_MESSAGE);
		}

		IEnumerable<UserBasicViewModel> unassignedTeachers = await this._teacherService.GetUnassignedTeachersAsync();
		IEnumerable<UserBasicViewModel> unassignedStudents = await this._studentService.GetUnassignedStudentsAsync();

		var viewModel = new AddClassroomViewModel
		{
			SchoolId = schoolId,
			UnassignedTeachers = unassignedTeachers,
			Students = unassignedStudents
		};


		return this.View(this.GetViewPath(nameof(this.Add)), viewModel);
	}

	[HttpPost]
	public async Task<IActionResult> Add(AddClassroomViewModel model)
	{
		if (this.ModelState.IsValid == false)
		{
			return this.View(this.GetViewPath(nameof(Add)), model);
		}

		try
		{
			bool addedSuccessfully = await this._classroomService.AddAsync(model);

			if (addedSuccessfully == false)
			{
				this.TempData["ErrorMessage"] = string.Format(ALREADY_ADDED_MESSAGE, CONTROLLER_NAME, model.Name);
			}
			else
			{
				this.TempData["SuccessMessage"] = string.Format(SUCCESSFULLY_ADDED_MESSAGE, CONTROLLER_NAME, model.Name);
			}

			return this.RedirectToAction(nameof(this.Manage));
		}
		catch (Exception)
		{
			this.ModelState.AddModelError(string.Empty, ADDITION_ERROR_MESSAGE);

			return this.View(this.GetViewPath(nameof(Add)), model);
		}
	}
}