using Server.Core.Commons.Datatables;
using Server.Core.Data.Repositories.Contracts;
using Server.Core.Modules.Product.Dto;

namespace Server.Core.Modules.Product.Repositories.Contracts;

public interface IProductRepo : IGenericRepository<Models.Product>
{
    IQueryable<Models.Product> FilterQuery(ProductFilter filter, DataTableFilter? dataTableFilter = null);
}
