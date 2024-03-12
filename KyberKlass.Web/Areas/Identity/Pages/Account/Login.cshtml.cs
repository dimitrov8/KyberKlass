#nullable disable

namespace KyberKlass.Web.Areas.Identity.Pages.Account;

using System.ComponentModel.DataAnnotations;
using Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class LoginModel : PageModel
{
	private readonly SignInManager<ApplicationUser> _signInManager;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly RoleManager<IdentityRole<Guid>> _roleManager;

	public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
	{
		this._signInManager = signInManager;
		this._userManager = userManager;
		this._roleManager = roleManager;
	}

	[BindProperty]
	public InputModel Input { get; set; }

	public string ReturnUrl { get; set; }

	[TempData]
	public string ErrorMessage { get; set; }

	public class InputModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}

	public async Task<IActionResult> OnGetAsync(string returnUrl = null)
	{
		if (!string.IsNullOrEmpty(this.ErrorMessage))
		{
			this.ModelState.AddModelError(string.Empty, this.ErrorMessage);
		}

		returnUrl ??= this.Url.Content("~/");

		if (this.User.Identity.IsAuthenticated)
		{
			return this.RedirectToAction("Index", "Home");
		}

		// Clear the existing external cookie to ensure a clean login process
		await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

		this.ReturnUrl = returnUrl;

		return this.Page();
	}

	public async Task<IActionResult> OnPostAsync(string returnUrl = null)
	{
		returnUrl ??= this.Url.Content("~/");

		if (this.ModelState.IsValid)
		{
			// This doesn't count login failures towards account lockout
			// To enable password failures to trigger account lockout, set lockoutOnFailure: true
			var result = await this._signInManager.PasswordSignInAsync(this.Input.Email, this.Input.Password, true, false);

			if (result.Succeeded)
			{
				var user = await this._userManager.FindByEmailAsync(this.Input.Email);

				if (user != null)
				{
					var roleName = (await this._userManager.GetRolesAsync(user)).FirstOrDefault();
					user.Role = await this._roleManager.FindByNameAsync(roleName);
				}

				return this.RedirectToAction("Index", "Home");
			}

			this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
			return this.Page();
		}

		// If we got this far, something failed, redisplay form
		return this.Page();
	}
}