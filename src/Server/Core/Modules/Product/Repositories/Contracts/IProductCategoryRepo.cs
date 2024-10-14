using Server.Core.Commons.Datatables;
using Server.Core.Data.Repositories.Contracts;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Models;

namespace Server.Core.Modules.Product.Repositories.Contracts;

public interface IProductCategoryRepo : IGenericRepository<ProductCategory>
{
    IQueryable<ProductCategory> FilterQuery(ProductCategoryFilter filter, DataTableFilter? dataTableFilter = null);
}
