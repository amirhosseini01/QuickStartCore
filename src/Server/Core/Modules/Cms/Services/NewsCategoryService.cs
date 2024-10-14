using Microsoft.AspNetCore.Mvc.Rendering;
using Server.Core.Commons;
using Server.Core.Commons.Datatables;
using Server.Core.Modules.Cms.Dto;
using Server.Core.Modules.Cms.Models;
using Server.Core.Modules.Cms.Repositories.Contracts;
using Server.Core.Modules.Cms.Mapper;

namespace Server.Core.Modules.Cms.Services;

public static class NewsCategoryService
{
    public static async Task<DataTableResult<NewsCategory>> GetDataTableAsync(this INewsCategoryRepo repo, NewsCategoryFilter filter, DataTableFilter dataTableFilter, CancellationToken ct = default)
    {
        var query = repo.FilterQuery(filter: filter, dataTableFilter: dataTableFilter)
            .OrderByDescending(x => x.ViewOrder).ThenByDescending(x => x.Id);

        return await query.ToDataTableAsync(dataTableFilter, cancellationToken: ct);
    }

    public static async Task<PaginatedList<SelectListItem>> GetSelectListItemsAsync(this INewsCategoryRepo repo, NewsCategoryFilter filter, PaginatedListFilter paginatedListFilter, CancellationToken ct = default)
    {
        var query = repo.FilterQuery(filter: filter)
            .OrderByDescending(x => x.ViewOrder).ThenByDescending(x => x.Id);

        return await PaginatedList<SelectListItem>.CreateAsync(
            source: query.MapEntityToSelectList(),
            paginatedListFilter: paginatedListFilter,
            ct: ct);
    }

    public static async Task<NewsCategory?> GetByIdAsync(this INewsCategoryRepo repo, IdDto route, CancellationToken ct = default)
    {
        return await repo.FindAsync(id: route.Id, ct: ct);
    }

    public static async Task<ResponseDto<string>> AddAsync(this INewsCategoryRepo repo, NewsCategory input, CancellationToken ct = default)
    {
        await repo.AddAsync(input, ct);
        await repo.SaveChangesAsync(ct);
        return ResponseBase.Success<string>();
    }

    public static async Task<ResponseDto<string>> UpdateAsync(this INewsCategoryRepo repo, IdDto route, NewsCategory input, CancellationToken ct = default)
    {
        var entity = await repo.GetByIdAsync(route, ct);
        if (entity is null)
        {
            return ResponseBase.Failed<string>(Messages.NotFound);
        }

        new NewsCategoryMapper().InputToEntity(input: input, entity: entity);

        await repo.SaveChangesAsync(ct);
        return ResponseBase.Success<string>();
    }

    public static async Task<ResponseDto<string>> RemoveAsync(this INewsCategoryRepo repo, IdDto route, CancellationToken ct = default)
    {
        var entity = await repo.GetByIdAsync(route, ct);
        if (entity is null)
        {
            return ResponseBase.Failed<string>(Messages.NotFound);
        }
        
        repo.Remove(entity);
        await repo.SaveChangesAsync(ct);
        return ResponseBase.Success<string>();
    }
}
