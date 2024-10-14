using Server.Core.Commons;
using Server.Core.Modules.Notification.Commons;
using Server.Core.Modules.Notification.Dto;
using Server.Core.Modules.Notification.Repositories.Contracts;
using Server.Core.Modules.Notification.Repositories.Queries;

namespace Server.Core.Modules.Notification.Services;

public static class NotificationLogService
{
	public static async Task<ResponseDto<string>> AddSmsLog(this INotificationLogRepo repo, NotificationLogAdminInputDto adminInput, CancellationToken ct = default)
	{
		if (await repo.ReachedDailySmsCountLimit(
				adminFilterDto3: new NotificationLogAdminFilterDto3()
				{
					UserId = adminInput.UserId,
					FromCreateDate = DateTime.Now.Date,
					UntilCreateDate = DateTime.Now.Date,
					Section = adminInput.Section,
					Type = adminInput.Type
				}, ct: ct))
		{
			return ResponseBase.Failed<string>(Messages.ReachedDailySmsCountLimit);
		}

		var entity = new Mapper.NotificationLogMapper().AdminInputToEntity(adminInput);

		await repo.AddAsync(entity, ct);
		await repo.SaveChangesAsync(ct);
		return ResponseBase.Success<string>();
	}

	public static async Task<ResponseDto<string>> CheckSmsVerifyCode(this INotificationLogRepo repo, string verifyCode, NotificationLogAdminFilterDto3 adminFilterDto3, CancellationToken ct = default)
	{
		var entity = await repo.GetByVerifyCode(adminFilterDto3: adminFilterDto3, ct: ct);
		if (entity is null)
		{
			return ResponseBase.Failed<string>(Messages.InvalidVerifyCode);
		}

		if (entity.Value != verifyCode)
		{
			return ResponseBase.Failed<string>(Messages.InvalidVerifyCode);
		}

		if (entity.CreateDateTime.AddMinutes(NotificationHelper.SmsVerifyCodeMinuteLimit) <= DateTime.Now)
		{
			return ResponseBase.Failed<string>(Messages.VerifyCodeExpired);
		}

		return ResponseBase.Success<string>();
	}
}
