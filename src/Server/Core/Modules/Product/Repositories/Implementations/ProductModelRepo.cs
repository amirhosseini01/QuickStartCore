using Microsoft.EntityFrameworkCore;
using Server.Core.Commons.Datatables;
using Server.Core.Data;
using Server.Core.Data.Repositories.Implementations;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Models;
using Server.Core.Modules.Product.Repositories.Contracts;

namespace Server.Core.Modules.Product.Repositories.Implementations;

public class ProductModelRepo(BaseDbContext context) : GenericRepository<ProductModel>(context), IProductModelRepo
{
    private readonly DbSet<ProductModel> _entities = context.ProductModels;

    public IQueryable<ProductModel> FilterQuery(ProductModelFilter filter, DataTableFilter? dataTableFilter = null)
    {
        var query = _entities.AsNoTracking();

        if (filter.ProductId > 0)
        {
            query = query.Where(x => x.ProductId == filter.ProductId);
        }

        if (dataTableFilter is not null)
        {
            query = FilterQuery(query, dataTableFilter);
        }

        return query;
    }

    private static IQueryable<ProductModel> FilterQuery(IQueryable<ProductModel> query, DataTableFilter filter)
    {
        if (filter.Search is not null && !string.IsNullOrEmpty(filter.Search.Value))
        {
            query = query.Where(x =>
                x.Title.Contains(filter.Search.Value) ||
                x.Product.Title.Contains(filter.Search.Value)
            );
        }

        return query;
    }
}