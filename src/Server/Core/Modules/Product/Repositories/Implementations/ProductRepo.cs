using Microsoft.EntityFrameworkCore;
using Server.Core.Commons.Datatables;
using Server.Core.Data;
using Server.Core.Data.Repositories.Implementations;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Repositories.Contracts;

namespace Server.Core.Modules.Product.Repositories.Implementations;

public class ProductRepo(BaseDbContext context) : GenericRepository<Models.Product>(context), IProductRepo
{
    private readonly DbSet<Models.Product> _entities = context.Products;

    public IQueryable<Models.Product> FilterQuery(ProductFilter filter, DataTableFilter? dataTableFilter = null)
    {
        var query = _entities.AsNoTracking();

        if (filter.Visible is not null)
        {
            query = query.Where(x => x.Visible == filter.Visible.Value);
            query = query.Where(x => x.ProductCategory.Visible == filter.Visible.Value);
            query = query.Where(x => x.ProductBrand.Visible == filter.Visible.Value);
        }

        if (filter.Saleable is not null)
        {
            query = query.Where(x => x.Saleable == filter.Saleable.Value);
        }

        if (filter.IsSpecialOffer is not null)
        {
            query = query.Where(x => x.IsSpecialOffer == filter.IsSpecialOffer.Value);
        }

        if (filter.HasDiscount == true)
        {
            query = query.Where(x => x.ProductModels.Count(xx => xx.Discount > 0) > 0);
        }
        // product stock (is product exist)
        if (filter.Available == true)
        {
            query = query.Where(x => x.ProductStocks.Sum(xx => xx.Value) > 0);
        }

        if (dataTableFilter is not null)
        {
            query = FilterQuery(query, dataTableFilter);
        }
        
        if (filter.ProductBrandId > 0)
        {
            query = query.Where(x => x.ProductBrandId == filter.ProductBrandId.Value);
        }

        if (filter.ProductCategoryId > 0)
        {
            query = query.Where(x => x.ProductCategoryId == filter.ProductCategoryId.Value);
        }    

        return query;
    }
    
    private static IQueryable<Models.Product> FilterQuery(IQueryable<Models.Product> query, DataTableFilter dataTableFilter)
    {
        if (dataTableFilter.Search is not null && !string.IsNullOrEmpty(dataTableFilter.Search?.Value))
        {
            query = query.Where(x => x.Title.Contains(dataTableFilter.Search.Value));
        }

        return query;
    }
}
