using Microsoft.AspNetCore.Mvc.Rendering;
using Server.Core.Commons;
using Server.Core.Commons.Datatables;
using Server.Core.Modules.User.Dto;
using Server.Core.Modules.User.Models;
using Server.Core.Modules.User.Repositories.Contracts;

namespace Server.Core.Modules.User.Services;

public static class UserService
{

    public static async Task<DataTableResult<AppUser>> GetDataTableAsync(this IUserRepo repo, UserFilter filter, DataTableFilter dataTableFilter, CancellationToken ct = default)
    {
        var query = repo.FilterQuery(filter: filter, dataTableFilter: dataTableFilter).OrderByDescending(x => x.Id);

        return await query.ToDataTableAsync(dataTableFilter, ct);
    }

    public static async Task<PaginatedList<SelectListItem>> GetSelectListItemsAsync(this IUserRepo repo, UserFilter filter, PaginatedListFilter paginatedListFilter, CancellationToken ct = default)
    {
        var query = repo.FilterQuery(filter: filter).OrderByDescending(x => x.Id);

        return await PaginatedList<SelectListItem>.CreateAsync(
            source: Mapper.UserMapperQuery.MapEntityToSelectList(query),
            paginatedListFilter: paginatedListFilter,
            ct: ct);
    }

    public static async Task<AppUser?> GetByIdAsync(this IUserRepo repo, IdDto route, CancellationToken ct = default)
    {
        return await repo.FindAsync(id: route.Id, ct: ct);
    }

    public static async Task<ResponseDto<string>> AddAsync(this IUserRepo repo, UserInput input, CancellationToken ct = default)
    {
        var user = new Mapper.UserMapper().InputToEntity(input);
        await repo.AddAsync(user, ct);
        await repo.SaveChangesAsync(ct);
        return ResponseBase.Success<string>();
    }

    public static async Task<ResponseDto<string>> UpdateAsync(this IUserRepo repo, IdDto route, UserInput input, CancellationToken ct = default)
    {
        var entity = await repo.GetByIdAsync(route: route, ct: ct);
        if (entity is null)
        {
            return ResponseBase.Failed<string>(Messages.NotFound);
        }
        
        new Mapper.UserMapper().InputToEntity(input, entity);
        await repo.SaveChangesAsync(ct);
        return ResponseBase.Success<string>();
    }

    public static async Task<ResponseDto<string>> RemoveAsync(this IUserRepo repo, IdDto route, CancellationToken ct = default)
    {
        var entity = await repo.GetByIdAsync(route: route, ct: ct);
        if (entity is null)
        {
            return ResponseBase.Failed<string>(Messages.NotFound);
        }
        
        repo.Remove(entity);
        await repo.SaveChangesAsync(ct);
        return ResponseBase.Success<string>();
    }
}
