using System.Globalization;

namespace Server.Core.Commons.Extensions;

public static class PersianDate
{
	public static string ToPersianDate(this DateTime dateTime)
	{
		var pc = new PersianCalendar();

		return $"{pc.GetYear(dateTime)}/{pc.GetMonth(dateTime)}/{pc.GetDayOfMonth(dateTime)}";
	}
}
