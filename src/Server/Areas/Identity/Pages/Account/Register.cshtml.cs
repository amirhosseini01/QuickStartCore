using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Core.Modules.User.Dto;
using Server.Core.Modules.User.Models;

namespace Server.Areas.Identity.Pages.Account;

public class RegisterModel(
	SignInManager<AppUser> signInManager,
	UserManager<AppUser> userManager
) : PageModel
{
	[BindProperty] public RegisterInput Input { get; set; }
	public string? ReturnUrl { get; set; }

	public async Task OnGetAsync(string? returnUrl = null)
	{
		ReturnUrl = returnUrl;
	}

	public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
	{
		returnUrl ??= Url.Content("~/");
		if (!ModelState.IsValid)
		{
			return Page();
		}

		var user = new AppUser { PhoneNumber = Input.PhoneNumber, UserName = Input.PhoneNumber };
		var result = await userManager.CreateAsync(user, Input.Password);
		if (!result.Succeeded)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}

			return Page();
		}
		
		if (userManager.Options.SignIn.RequireConfirmedPhoneNumber)
		{
			return RedirectToPage("RegisterConfirmation",
				new { phoneNumber = Input.PhoneNumber, returnUrl });
		}
		await signInManager.SignInAsync(user, isPersistent: false);
		return LocalRedirect(returnUrl);
	}
}
