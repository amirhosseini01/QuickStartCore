using Riok.Mapperly.Abstractions;
using Server.Core.Modules.Notification.Dto;
using Server.Core.Modules.Notification.Models;

namespace Server.Core.Modules.Notification.Mapper;

[Mapper]
public partial class NotificationLogMapper
{
	public partial NotificationLog AdminInputToEntity(NotificationLogAdminInputDto adminInput);	
}
