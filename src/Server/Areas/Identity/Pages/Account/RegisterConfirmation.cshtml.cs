using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Core.Modules.Notification.Commons;
using Server.Core.Modules.Notification.Dto;
using Server.Core.Modules.Notification.Enums;
using Server.Core.Modules.Notification.Repositories.Contracts;
using Server.Core.Modules.Notification.Services;
using Server.Core.Modules.User.Dto;
using Server.Core.Modules.User.Models;

namespace Server.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class RegisterConfirmationModel(
	SignInManager<AppUser> signInManager,
	UserManager<AppUser> userManager,
	INotificationLogRepo notificationLogRepo) : PageModel
{
	public string? ReturnUrl { get; set; }
	public RegisterConfirmationInputDto Input { get; set; }

	public async Task<IActionResult> OnGetAsync(string? phoneNumber, string? returnUrl = null,
		CancellationToken ct = default)
	{
		if (phoneNumber == null)
		{
			return RedirectToPage("/Index");
		}

		ReturnUrl = returnUrl ?? Url.Content("~/");

		var user = await userManager.FindByNameAsync(phoneNumber);
		if (user == null)
		{
			return NotFound($"کاربری با شماره تلفن {phoneNumber} یافت نشد. ");
		}

		Input = new()
		{
			PhoneNumber = user.PhoneNumber
		};

		// todo: send sms
		var result = await notificationLogRepo.AddSmsLog(
			adminInput: new NotificationLogAdminInputDto
			{
				UserId = user.Id,
				Receiver = user.PhoneNumber,
				Type = NotificationType.Sms,
				Section = NotificationSection.RegisterConfirmation,
				Value = NotificationHelper.GetVerifyCode()
			}, ct: ct);
		if (result.IsFailed)
		{
			foreach (var item in result.Messages)
			{
				ModelState.AddModelError("", item);	
			}
			
			return Page();
		}

		return Page();
	}

	public async Task<IActionResult> OnPostAsync(RegisterConfirmationInputDto input, string? returnUrl = null,
		CancellationToken ct = default)
	{
		if (!ModelState.IsValid)
		{
			Input = input;
			return Page();
		}

		var user = await userManager.FindByNameAsync(input.PhoneNumber);
		if (user == null)
		{
			return NotFound($"کاربری با شماره تلفن {input.PhoneNumber} یافت نشد. ");
		}

		var validateVerifyCodeRes = await notificationLogRepo.CheckSmsVerifyCode(verifyCode: input.VerifyCode,
			adminFilterDto3: new NotificationLogAdminFilterDto3()
			{
				Type = NotificationType.Sms,
				Section = NotificationSection.RegisterConfirmation,
				UserId = user.Id,
				FromCreateDate = DateTime.Now,
				UntilCreateDate = DateTime.Now,
			}, ct: ct
		);
		if (validateVerifyCodeRes.IsFailed)
		{
			foreach (var item in validateVerifyCodeRes.Messages)
			{
				ModelState.AddModelError("", item);
			}
			Input = input;
			return Page();
		}
		
		await signInManager.SignInAsync(user, isPersistent: false);
		return LocalRedirect(returnUrl ?? Url.Content("~/"));
	}
}
