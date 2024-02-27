namespace KyberKlass.Web.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Data.Interfaces;
using ViewModels.Admin.School;

[Authorize(Roles = "Admin")]
public class SchoolController : Controller
{
	private readonly ISchoolService _schoolService;

	public SchoolController(ISchoolService schoolService)
	{
		this._schoolService = schoolService;
	}

	private string GetViewPath(string viewName)
	{
		return $"~/Views/Admin/School/{viewName}.cshtml";
	}

	// Shows all schools
	[HttpGet]
	public async Task<IActionResult> All()
	{
		IEnumerable<SchoolViewModel> schools = await this._schoolService.AllSchoolsAsync();

		return this.View(this.GetViewPath(nameof(this.All)), schools);
	}

	//[HttpGet]
	//public async Task<IActionResult> Details(Guid? id)
	//{
	//	if (id == null)
	//	{
	//		return this.NotFound();
	//	}

	//	var school = await this._context.Schools
	//		.FirstOrDefaultAsync(m => m.Id == id);

	//	if (school == null)
	//	{
	//		return this.NotFound();
	//	}

	//	return this.View(school);
	//}

	// Returns a view from which we can add a school
	[HttpGet]
	public IActionResult Add()
	{
		return this.View(this.GetViewPath(nameof(Add)));
	}

	// Tries to add the school
	[HttpPost]
	public async Task<IActionResult> Add(AddSchoolFormModel model)
	{
		if (!this.ModelState.IsValid)
		{
			return this.View(this.GetViewPath(nameof(Add)), model);
		}

		try
		{
			bool addedSuccessfully = await this._schoolService.AddSchoolAsync(model);

			if (addedSuccessfully == false)
			{
				this.TempData["ErrorMessage"] = "School already added"!;
			}

			return this.RedirectToAction(nameof(this.All), model);
		}
		catch (Exception)
		{
			this.ModelState.AddModelError(string.Empty, "An error occurred while adding the school.");

			return this.View(this.GetViewPath(nameof(Add)), model);
		}
	}

	// Gets school by id
	[HttpGet]
	public async Task<IActionResult> Edit(string? id)
	{
		if (id == null)
		{
			return this.NotFound();
		}

		var school = await this._schoolService.GetForEditSchoolAsync(id);

		if (school == null)
		{
			return this.NotFound();
		}

		return this.View(this.GetViewPath(nameof(this.Edit)), school);
	}

	[HttpPost]
	public async Task<IActionResult> Edit(string id, SchoolViewModel model)
	{
		if (id != model.Id)
		{
			return this.NotFound();
		}

		if (!this.ModelState.IsValid)
		{
			return this.View(this.GetViewPath(nameof(Edit)), model);
		}

		try
		{
			await this._schoolService.EditSchoolAsync(id, model);

			return this.RedirectToAction(nameof(this.All));
		}
		catch (Exception)
		{
			return this.NotFound();
		}
	}

	//[HttpGet]
	//public async Task<IActionResult> Delete(Guid? id)
	//{
	//	if (id == null)
	//	{
	//		return this.NotFound();
	//	}

	//	var school = await this._context.Schools
	//		.FirstOrDefaultAsync(m => m.Id == id);

	//	if (school == null)
	//	{
	//		return this.NotFound();
	//	}

	//	return this.View(school);
	//}

	//[HttpPost]
	//[ActionName("Delete")]
	//[ValidateAntiForgeryToken]
	//public async Task<IActionResult> DeleteConfirmed(Guid id)
	//{
	//	var school = await this._context.Schools.FindAsync(id);

	//	if (school != null)
	//	{
	//		this._context.Schools.Remove(school);
	//	}

	//	await this._context.SaveChangesAsync();
	//	return this.RedirectToAction(nameof(this.All));
	//}

	//private bool SchoolExists(Guid id)
	//{
	//	return (this._context.Schools?.Any(e => e.Id == id)).GetValueOrDefault();
	//}
}