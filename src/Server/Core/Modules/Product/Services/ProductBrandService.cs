using Microsoft.AspNetCore.Mvc.Rendering;
using Server.Core.Commons;
using Server.Core.Commons.Datatables;
using Server.Core.Commons.UploadFile;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Models;
using Server.Core.Modules.Product.Repositories.Contracts;

namespace Server.Core.Modules.Product.Services;

public static class ProductBrandService
{
    public static async Task<DataTableResult<ProductBrand>> GetDataTableAsync(this IProductBrandRepo repo, ProductBrandFilter filter, DataTableFilter dataTableFilter , CancellationToken ct = default)
    {
        var query = repo.FilterQuery(filter: filter, dataTableFilter: dataTableFilter)
            .OrderByDescending(x => x.ViewOrder).ThenByDescending(x => x.Id);

        return await query.ToDataTableAsync(dataTableFilter, cancellationToken: ct);
    }

    public static async Task<PaginatedList<SelectListItem>> GetSelectListItemsAsync(this IProductBrandRepo repo, ProductBrandFilter filter, PaginatedListFilter paginatedListFilter, CancellationToken ct = default)
    {
        var query = repo.FilterQuery(filter: filter)
            .OrderByDescending(x => x.ViewOrder).ThenByDescending(x => x.Id);

        return await PaginatedList<SelectListItem>.CreateAsync(
            source: Mapper.ProductBrandMapperQuery.MapEntityToSelectList(query),
            paginatedListFilter: paginatedListFilter,
            ct: ct);
    }

    public static async Task<ProductBrand?> GetByIdAsync(this IProductBrandRepo repo, IdDto route, CancellationToken ct = default)
    {
        return await repo.FindAsync(id: route.Id, ct: ct);
    }

    public static async Task<ResponseDto<string>> AddAsync(this IProductBrandRepo repo, FileUploader fileUploader, ProductBrandInput input, CancellationToken ct = default)
    {
        var entity = new Mapper.ProductBrandMapper().InputToEntity(input);

        if (input.LogoFile is not null)
        {
            var uploadRes = await fileUploader.UploadFile(input.LogoFile);
            if (uploadRes.IsFailed)
            {
                return uploadRes;
            }
            entity.Logo = uploadRes.Obj;
        }

        await repo.AddAsync(entity, ct);
        await repo.SaveChangesAsync(ct);
        return ResponseBase.Success<string>();
    }

    public static async Task<ResponseDto<string>> UpdateAsync(this IProductBrandRepo repo, FileUploader fileUploader, IdDto route, ProductBrandInput input, CancellationToken ct = default)
    {
        var entity = await repo.GetByIdAsync(route: route, ct: ct);
        if (entity is null)
        {
            return ResponseBase.Failed<string>(message: Messages.NotFound);
        }
        
        new Mapper.ProductBrandMapper().InputToEntity(input, entity);

        string? previousLogo = null;
        if (input.LogoFile is not null)
        {
            var uploadRes = await fileUploader.UploadFile(input.LogoFile);
            if (uploadRes.IsFailed)
            {
                return uploadRes;
            }
            previousLogo = entity.Logo;
            entity.Logo = uploadRes.Obj;
        }

        await repo.SaveChangesAsync(ct);
        if (previousLogo is not null)
        {
            fileUploader.DeleteFile(previousLogo);
        }
        return ResponseBase.Success<string>();
    }

    public static async Task<ResponseDto<string>> RemoveAsync(this IProductBrandRepo repo, FileUploader fileUploader, IdDto route, CancellationToken ct = default)
    {
        var entity = await repo.GetByIdAsync(route: route, ct: ct);
        if (entity is null)
        {
            return ResponseBase.Failed<string>(message: Messages.NotFound);
        }
        
        repo.Remove(entity);
        await repo.SaveChangesAsync(ct);
        if (entity.Logo is not null)
        {
            fileUploader.DeleteFile(entity.Logo);
        }
        
        return ResponseBase.Success<string>();
    }
}
