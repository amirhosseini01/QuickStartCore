using Server.Core.Data.Repositories.Contracts;
using Server.Core.Modules.Notification.Dto;
using Server.Core.Modules.Notification.Models;

namespace Server.Core.Modules.Notification.Repositories.Contracts;

public interface INotificationLogRepo: IGenericRepository<NotificationLog>
{
	IQueryable<NotificationLog> FilterQuery(NotificationLogAdminFilterDto3 adminFilterDto3);
}
