using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Core.Commons;
using Server.Core.Data;
using Server.Core.Modules.Notification.Enums;
using Server.Core.Modules.User.Models;

namespace Server.Core.Modules.Notification.Models;

public class NotificationLog: BaseEntity
{
	public int? UserId { get; set; }
	
	[Required]
	[Column(TypeName = ModelStatics.Nvarchar50)]
	public NotificationType Type { get; set; }
	
	[Required]
	[Column(TypeName = ModelStatics.Nvarchar50)]
	public NotificationSection Section { get; set; }

	[Required]
	[StringLength(ModelStatics.TitleRequiredLength)]
	public string Receiver { get; set; }
	
	[Required]
	[StringLength(ModelStatics.TitleRequiredLength)]
	public string Value { get; set; }

	public DateTime CreateDateTime { get; set; } = DateTime.Now;

	[ForeignKey(nameof(UserId))]
	public AppUser User { get; set; }
}
