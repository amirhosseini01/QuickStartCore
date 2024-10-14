using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Core.Commons;
using Server.Core.Modules.User.Dto;
using Server.Core.Modules.User.Models;

namespace Server.Areas.Identity.Pages.Account;

public class LoginModel(SignInManager<AppUser> signInManager) : PageModel
{
	[BindProperty] public LoginInput Input { get; set; }
	public string? ReturnUrl { get; set; }
	public async Task OnGetAsync(string? returnUrl = null)
	{
		returnUrl ??= Url.Content("~/");

		// Clear the existing external cookie to ensure a clean login process
		await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

		ReturnUrl = returnUrl;
	}

	public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
	{
		returnUrl ??= Url.Content("~/");

		if (!ModelState.IsValid)
		{
			return Page();
		}

		var result = await signInManager.PasswordSignInAsync(Input.PhoneNumber, Input.Password, Input.RememberMe,
			lockoutOnFailure: true);
		if (result.Succeeded)
		{
			return LocalRedirect(returnUrl);
		}

		if (result.RequiresTwoFactor)
		{
			return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
		}

		if (result.IsLockedOut)
		{
			return RedirectToPage("./Lockout");
		}

		ModelState.AddModelError(string.Empty, Messages.SignInFailedError);
		return Page();
	}
}
