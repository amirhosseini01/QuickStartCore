using Server.Core.Commons.Datatables;
using Server.Core.Data.Repositories.Contracts;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Models;

namespace Server.Core.Modules.Product.Repositories.Contracts;

public interface IProductBrandRepo : IGenericRepository<ProductBrand>
{
    IQueryable<ProductBrand> FilterQuery(ProductBrandFilter filter, DataTableFilter? dataTableFilter = null);
}