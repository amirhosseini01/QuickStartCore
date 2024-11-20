using Microsoft.EntityFrameworkCore;
using Server.Core.Commons.Datatables;
using Server.Core.Data;
using Server.Core.Data.Repositories.Implementations;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Models;
using Server.Core.Modules.Product.Repositories.Contracts;

namespace Server.Core.Modules.Product.Repositories.Implementations;

public class ProductCategoryRepo(BaseDbContext context) : GenericRepository<ProductCategory>(context), IProductCategoryRepo
{
    private readonly DbSet<ProductCategory> _entities = context.ProductCategories;

    public IQueryable<ProductCategory> FilterQuery(ProductCategoryFilter filter, DataTableFilter? dataTableFilter = null)
    {
        var query = _entities.AsNoTracking();

        if (filter.Visible is not null)
        {
            query = query.Where(x => x.Visible == filter.Visible.Value);
        }

        if (dataTableFilter is not null)
        {
            query = FilterQuery(query, dataTableFilter);
        }

        return query;
    }

    private static IQueryable<ProductCategory> FilterQuery(IQueryable<ProductCategory> query, DataTableFilter filter)
    {
        if (filter.Search is not null && !string.IsNullOrEmpty(filter.Search.Value))
        {
            query = query.Where(x =>
                x.Title.Contains(filter.Search.Value)
            );
        }

        return query;
    }
}
