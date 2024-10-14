using System.ComponentModel.DataAnnotations;
using Server.Core.Commons;

namespace Server.Core.Modules.Notification.Enums;

public enum NotificationType
{
	[Display(Name = Messages.Sms)] Sms
}
