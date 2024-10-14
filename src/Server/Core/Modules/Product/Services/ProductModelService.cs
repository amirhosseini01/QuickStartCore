using Microsoft.AspNetCore.Mvc.Rendering;
using Server.Core.Commons;
using Server.Core.Commons.Datatables;
using Server.Core.Data.Models;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Models;
using Server.Core.Modules.Product.Repositories.Contracts;

namespace Server.Core.Modules.Product.Services;

public static class ProductModelService
{
    public static async Task<DataTableResult<ProductModel>> GetDataTableAsync(this IProductModelRepo repo, ProductModelFilter filter, DataTableFilter dataTableFilter, CancellationToken ct = default)
    {
        var query = repo.FilterQuery(filter: filter, dataTableFilter: dataTableFilter);

        return await query.ToDataTableAsync(dataTableFilter, ct);
    }
    
    public static async Task<PaginatedList<SelectListItem>> GetSelectListItemsAsync(this IProductModelRepo repo, ProductModelFilter filter, PaginatedListFilter paginatedListFilter, CancellationToken ct = default)
    {
        var query = repo.FilterQuery(filter: filter)
            .OrderByDescending(x => x.Id);

        return await PaginatedList<SelectListItem>.CreateAsync(
            source: Mapper.ProductModelMapperQuery.MapEntityToSelectList(query),
            paginatedListFilter,
            ct: ct);
    }

    public static async Task<ProductModel?> GetByIdAsync(this IProductModelRepo repo, IdDto route, CancellationToken ct = default)
    {
        var filterQuery = new BaseQueryFilter()
        {
            Includes = [nameof(ProductModel.Product)]
        };
        return await repo.FirstOrDefaultAsync(predicate: x => x.Id == route.Id, filterQuery: filterQuery, ct: ct);
    }

    public static async Task<ResponseDto<string>> AddAsync(this IProductModelRepo repo, ProductModel input, CancellationToken ct = default)
    {
        await repo.AddAsync(item: input, ct);
        await repo.SaveChangesAsync(ct);
        return ResponseBase.Success<string>();
    }

    public static async Task<ResponseDto<string>> UpdateAsync(this IProductModelRepo repo, IdDto route, ProductModel input, CancellationToken ct = default)
    {
        var entity = await repo.FindAsync(id: route.Id, ct: ct);
        if (entity is null)
        {
            return ResponseBase.Failed<string>(message: Messages.NotFound);
        }
        
        new Mapper.ProductModelMapper().InputToEntity(input, entity);

        await repo.SaveChangesAsync(ct);
        return ResponseBase.Success<string>();
    }

    public static async Task<ResponseDto<string>> RemoveAsync(this IProductModelRepo repo, IdDto route, CancellationToken ct = default)
    {
        var entity = await repo.GetByIdAsync(route: route, ct: ct);
        if (entity is null)
        {
            return ResponseBase.Failed<string>(message: Messages.NotFound);
        }
        
        repo.Remove(item: entity);
        await repo.SaveChangesAsync(ct);
        return ResponseBase.Success<string>();
    }
}
