using System.Text;
using Server.Core.Commons;

namespace Server.Core.Modules.Notification.Commons;

public static class NotificationHelper
{
	public const int DailySmsCountLimit = 5;
	public const int SmsVerifyCodeMinuteLimit = 5;
	public static string GetVerifyCode()
	{
		//todo: use secure random generator
		// var r = RandomNumberGenerator.Create();
		var r = new Random();
		var sb = new StringBuilder();
		for (var i = 0; i < ModelStatics.VerifyCodeRequiredLength; i++)
		{
			sb.Append(r.Next(minValue: 1, maxValue: 9));
		}

		return sb.ToString();
	}
}
