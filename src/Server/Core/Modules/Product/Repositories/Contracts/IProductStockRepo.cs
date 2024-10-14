using Server.Core.Commons.Datatables;
using Server.Core.Data.Repositories.Contracts;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Models;

namespace Server.Core.Modules.Product.Repositories.Contracts;

public interface IProductStockRepo : IGenericRepository<ProductStock>
{
    IQueryable<ProductStock> FilterQuery(ProductStockFilter filter, DataTableFilter? dataTableFilter = null);
}
