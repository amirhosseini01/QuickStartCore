using Microsoft.EntityFrameworkCore;
using Server.Core.Commons.Datatables;
using Server.Core.Data;
using Server.Core.Data.Repositories.Implementations;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Models;
using Server.Core.Modules.Product.Repositories.Contracts;

namespace Server.Core.Modules.Product.Repositories.Implementations;

public class ProductSellerRepo(BaseDbContext context) : GenericRepository<ProductSeller>(context), IProductSellerRepo
{
    private readonly DbSet<ProductSeller> _entities = context.ProductSellers;
    public IQueryable<ProductSeller> FilterQuery(ProductSellerFilter filter, DataTableFilter? dataTableFilter = null)
    {
        var query = _entities.AsNoTracking();
        
        if (filter.UserId is not null)
        {
            return query.Where(x => x.UserId == filter.UserId);
        }
        
        if (dataTableFilter is not null)
        {
            query = FilterQuery(query, dataTableFilter);
        }

        return query;
    }
    
    private static IQueryable<ProductSeller> FilterQuery(IQueryable<ProductSeller> query, DataTableFilter dataTableFilter)
    {
        if (!string.IsNullOrEmpty(dataTableFilter.Search?.Value))
        {
            query = query.Where(x => x.Title.Contains(dataTableFilter.Search.Value));
        }

        return query;
    }
}
