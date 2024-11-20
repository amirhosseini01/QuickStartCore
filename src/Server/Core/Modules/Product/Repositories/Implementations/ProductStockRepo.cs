using Microsoft.EntityFrameworkCore;
using Server.Core.Commons.Datatables;
using Server.Core.Data;
using Server.Core.Data.Repositories.Implementations;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Models;
using Server.Core.Modules.Product.Repositories.Contracts;

namespace Server.Core.Modules.Product.Repositories.Implementations;

public class ProductStockRepo(BaseDbContext context) : GenericRepository<ProductStock>(context), IProductStockRepo
{
    private readonly DbSet<ProductStock> _entities = context.ProductStocks;

    public IQueryable<ProductStock> FilterQuery(ProductStockFilter filter, DataTableFilter? dataTableFilter = null)
    {
        var query = _entities.AsNoTracking();

        if (filter.ProductModelId > 0)
        {
            query = query.Where(x => x.ProductModelId == filter.ProductModelId);
        }

        if (filter.ProductId > 0)
        {
            query = query.Where(x => x.ProductId == filter.ProductId);
        }    

        if (dataTableFilter is not null)
        {
            if (dataTableFilter.Search is not null && !string.IsNullOrEmpty(dataTableFilter.Search.Value))
            {
                query = query.Where(x =>
                    x.Product.Title.Contains(dataTableFilter.Search.Value) ||
                    x.ProductModel.Title.Contains(dataTableFilter.Search.Value)
                );
            }
        }

        return query;
    }
}
