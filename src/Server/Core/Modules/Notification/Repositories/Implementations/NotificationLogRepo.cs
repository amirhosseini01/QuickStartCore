using Microsoft.EntityFrameworkCore;
using Server.Core.Data;
using Server.Core.Data.Repositories.Implementations;
using Server.Core.Modules.Notification.Dto;
using Server.Core.Modules.Notification.Models;
using Server.Core.Modules.Notification.Repositories.Contracts;

namespace Server.Core.Modules.Notification.Repositories.Implementations;

public class NotificationLogRepo(BaseDbContext context) : GenericRepository<NotificationLog>(context), INotificationLogRepo
{
	private readonly DbSet<NotificationLog> entities = context.NotificationLogs;
	public IQueryable<NotificationLog> FilterQuery(NotificationLogAdminFilterDto3 adminFilterDto3)
	{
		var query = entities.AsNoTracking();

		if (adminFilterDto3.UserId is not null)
		{
			query = query.Where(x => x.UserId == adminFilterDto3.UserId);
		}
		
		if (adminFilterDto3.FromCreateDate is not null)
		{
			query = query.Where(x => x.CreateDateTime.Date >= adminFilterDto3.FromCreateDate.Value.Date);
		}
		
		if (adminFilterDto3.UntilCreateDate is not null)
		{
			query = query.Where(x => x.CreateDateTime.Date <= adminFilterDto3.UntilCreateDate.Value.Date);
		}
		
		if (adminFilterDto3.Type is not null)
		{
			query = query.Where(x => x.Type == adminFilterDto3.Type.Value);
		}
		
		if (adminFilterDto3.Section is not null)
		{
			query = query.Where(x => x.Section == adminFilterDto3.Section.Value);
		}

		return query;
	}
}
