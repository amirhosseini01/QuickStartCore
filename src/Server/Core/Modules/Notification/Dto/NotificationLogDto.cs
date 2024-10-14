using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Server.Core.Commons;
using Server.Core.Modules.Notification.Enums;

namespace Server.Core.Modules.Notification.Dto;

public class NotificationLogAdminInputDto
{
	[DisplayName(Messages.User)]
	[Range(minimum: ModelStatics.MinimumIdRange, maximum: ModelStatics.MaximumIdRange, ErrorMessage = Messages.RangeModelState)]
	public int? UserId { get; set; }
	
	[DisplayName(Messages.Type)]
	[Required(ErrorMessage = Messages.RequiredModelState)]
	public NotificationType Type { get; set; }
	
	[DisplayName(Messages.Section)]
	[Required(ErrorMessage = Messages.RequiredModelState)]
	public NotificationSection Section { get; set; }

	[DisplayName(Messages.Receiver)]
	[Required(ErrorMessage = Messages.RequiredModelState)]
	[MaxLength(ModelStatics.TitleRequiredLength)]
	[MinLength(ModelStatics.TitleMinimumLength)]
	public string Receiver { get; set; }
	
	[DisplayName(Messages.Value)]
	[Required(ErrorMessage = Messages.RequiredModelState)]
	[MaxLength(ModelStatics.TitleRequiredLength)]
	[MinLength(ModelStatics.TitleMinimumLength)]
	public string Value { get; set; }
}

public class NotificationLogAdminFilterDto3
{
	public int? UserId { get; set; }
	public DateTime? FromCreateDate { get; set; }
	public DateTime? UntilCreateDate { get; set; }
	public NotificationType? Type { get; set; }
	public NotificationSection? Section { get; set; }
}
