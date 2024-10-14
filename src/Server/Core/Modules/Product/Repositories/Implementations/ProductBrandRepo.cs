using Microsoft.EntityFrameworkCore;
using Server.Core.Commons.Datatables;
using Server.Core.Data;
using Server.Core.Data.Repositories.Implementations;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Models;
using Server.Core.Modules.Product.Repositories.Contracts;

namespace Server.Core.Modules.Product.Repositories.Implementations;

public class ProductBrandRepo(BaseDbContext context) : GenericRepository<ProductBrand>(context), IProductBrandRepo
{
	private readonly DbSet<ProductBrand> _entities = context.ProductBrands;

	public IQueryable<ProductBrand> FilterQuery(ProductBrandFilter filter, DataTableFilter? dataTableFilter = null)
	{
		var query = _entities.AsNoTracking();

		if (filter.Visible is not null)
		{
			query = query.Where(x => x.Visible == filter.Visible.Value);
		}

		if (dataTableFilter is not null)
		{
			query = FilterQuery(query: query, dataTableFilter: dataTableFilter);
		}

		return query;
	}

	private static IQueryable<ProductBrand> FilterQuery(IQueryable<ProductBrand> query, DataTableFilter dataTableFilter)
	{
		if (!string.IsNullOrEmpty(dataTableFilter.Search?.Value))
		{
			query = query.Where(x => x.Title.Contains(dataTableFilter.Search.Value));
		}

		return query;
	}
}
