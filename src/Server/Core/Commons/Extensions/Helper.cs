using System.Globalization;

namespace Server.Core.Commons.Extensions;

public static class Helper
{
	public static string ShowPrice(this decimal? value)
	{
		if (value is null || value <= 0)
		{
			return Messages.Unavailable;
		}

		return value.Value.ShowPrice();
	}
	public static string ShowPrice(this decimal value)
	{
		return value.ToThreeDigit();
	}

	public static string ToThreeDigit(this decimal value)
	{
		return value.ToString("N0");
	}

	public static decimal GetPercent(this decimal? discount, decimal? price)
	{
		if (price is null || price <= 0 || discount is null || discount <= 0)
		{
			return 0;
		}

		return discount.Value * 100 / price.Value;
	}

	public static int GetPersianYear(this DateTime date)
	{
		var pc = new PersianCalendar();
		return pc.GetYear(date);
	}
	
	public static bool IsActiveMenus(this string path, List<string> subMenus)
	{
		return subMenus.Any(path.IsActiveMenu);
	}
	
	public static bool IsActiveMenu(this string path, string menu)
	{
		return path.Equals(menu, StringComparison.OrdinalIgnoreCase);
	}
}
