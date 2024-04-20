#nullable disable

namespace KyberKlass.Web.Areas.Identity.Pages.Account;

using System.ComponentModel.DataAnnotations;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Common.EntityValidations.BaseUser;

public class RegisterModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;

    public RegisterModel(
        UserManager<ApplicationUser> userManager,
        IUserStore<ApplicationUser> userStore,
        SignInManager<ApplicationUser> signInManager)
    {
        this._userManager = userManager;
        this._userStore = userStore;
        this._signInManager = signInManager;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public string ReturnUrl { get; set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(MAX_NAME_LENGTH, MinimumLength = MIN_NAME_LENGTH)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(MAX_NAME_LENGTH, MinimumLength = MIN_NAME_LENGTH)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required]
        [Display(Name = "Address")]
        [StringLength(MAX_ADDRESS_LENGTH, MinimumLength = MIN_ADDRESS_LENGTH)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(string returnUrl = null)
    {
        this.ReturnUrl = returnUrl;

        if (this.User.Identity?.IsAuthenticated ?? false)
        {
            return this.RedirectToAction("Index", "Home");
        }

        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= this.Url.Content("~/");

        if (this.ModelState.IsValid)
        {
            var user = this.CreateUser();

            await this._userStore.SetUserNameAsync(user, this.Input.Email, CancellationToken.None);
            var result = await this._userManager.CreateAsync(user, this.Input.Password);

            if (result.Succeeded)
            {
                await this._signInManager.SignInAsync(user, false);
                return this.LocalRedirect(returnUrl);
            }

            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return this.Page();
    }

    private ApplicationUser CreateUser()
    {
        try
        {
            var user = new ApplicationUser
            {
                FirstName = this.Input.FirstName,
                LastName = this.Input.LastName,
                BirthDate = this.Input.BirthDate,
                Address = this.Input.Address,
                UserName = this.Input.Email,
                Email = this.Input.Email,
                PhoneNumber = this.Input.PhoneNumber
            };

            return user;
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                                                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }
}