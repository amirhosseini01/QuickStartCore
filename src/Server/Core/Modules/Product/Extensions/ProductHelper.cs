using Server.Core.Modules.Product.Models;

namespace Server.Core.Modules.Product.Extensions;

public static class ProductHelper
{
	public static decimal GetBestPrice(this List<ProductModel>? productModels)
	{
		if (productModels is null || productModels.Count == 0)
		{
			return 0;
		}

		return productModels.Select(x=> x.FinalPrice).Order().First();
	}

	public static decimal GetFinalPrice(decimal price, decimal? discount)
	{
		if (discount is null or <= 0)
		{
			return 0;
		}
		return price - discount.Value;
	}
}
