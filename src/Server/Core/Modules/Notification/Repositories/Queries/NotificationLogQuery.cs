using Microsoft.EntityFrameworkCore;
using Server.Core.Modules.Notification.Commons;
using Server.Core.Modules.Notification.Dto;
using Server.Core.Modules.Notification.Models;
using Server.Core.Modules.Notification.Repositories.Contracts;

namespace Server.Core.Modules.Notification.Repositories.Queries;

public static class NotificationLogQuery
{
	public static async Task<bool> ReachedDailySmsCountLimit(this INotificationLogRepo repo, NotificationLogAdminFilterDto3 adminFilterDto3, CancellationToken ct = default)
	{
		return await repo.FilterQuery(adminFilterDto3: adminFilterDto3).CountAsync(ct) >= NotificationHelper.DailySmsCountLimit;
	}

	public static async Task<NotificationLog?> GetByVerifyCode(this INotificationLogRepo repo, NotificationLogAdminFilterDto3 adminFilterDto3, CancellationToken ct = default)
	{
		return await repo.FilterQuery(adminFilterDto3: adminFilterDto3).OrderByDescending(x => x.Id).FirstOrDefaultAsync(ct);
	}
}
