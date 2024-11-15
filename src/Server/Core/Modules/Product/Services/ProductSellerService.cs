using Microsoft.AspNetCore.Mvc.Rendering;
using Server.Core.Commons;
using Server.Core.Commons.Datatables;
using Server.Core.Commons.UploadFile;
using Server.Core.Data.Models;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Models;
using Server.Core.Modules.Product.Repositories.Contracts;

namespace Server.Core.Modules.Product.Services;

public static class ProductSellerService
{
    public static async Task<DataTableResult<ProductSeller>> GetDataTableAsync(this IProductSellerRepo repo, ProductSellerFilter filter, DataTableFilter dataTableFilter, CancellationToken ct = default)
    {
        var query = repo.FilterQuery(filter: filter, dataTableFilter: dataTableFilter).OrderByDescending(x => x.Id);

        return await query.ToDataTableAsync(dataTableFilter, ct);
    }

    public static async Task<PaginatedList<SelectListItem>> GetSelectListItemsAsync(this IProductSellerRepo repo, ProductSellerFilter filter, PaginatedListFilter paginatedListFilter, CancellationToken ct = default)
    {
        var query = repo.FilterQuery(filter: filter).OrderByDescending(x => x.Id);

        return await PaginatedList<SelectListItem>.CreateAsync(
            source: Mapper.ProductSellerMapperQuery.MapEntityToSelectList(query),
            paginatedListFilter: paginatedListFilter,
            ct: ct);
    }

    public static async Task<ProductSeller?> GetByIdAsync(this IProductSellerRepo repo, IdDto route, CancellationToken ct = default)
    {
        var filterQuery = new BaseQueryFilter()
        {
            Includes = [nameof(ProductSeller.User)]
        };
        return await repo.FirstOrDefaultAsync(predicate: x => x.Id == route.Id, filterQuery: filterQuery, ct: ct);
    }

    public static async Task<ResponseDto<string>> AddAsync(this IProductSellerRepo repo, FileUploader fileUploader, ProductSellerInput input, CancellationToken ct = default)
    {
        var entity = new Mapper.ProductSellerMapper().InputToEntity(input);

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

    public static async Task<ResponseDto<string>> UpdateAsync(this IProductSellerRepo repo, FileUploader fileUploader, IdDto route, ProductSellerInput input, CancellationToken ct = default)
    {
        var entity = await repo.FindAsync(id: route.Id, ct: ct);
        if (entity is null)
        {
            return ResponseBase.Failed<string>(message: Messages.NotFound);
        }
        
        new Mapper.ProductSellerMapper().InputToEntity(input, entity);

        string? previous = null;
        if (input.LogoFile is not null)
        {
            var uploadRes = await fileUploader.UploadFile(input.LogoFile);
            if (uploadRes.IsFailed)
            {
                return uploadRes;
            }

            previous = entity.Logo;
            entity.Logo = uploadRes.Obj;
        }

        await repo.SaveChangesAsync(ct);

        if (previous is not null)
        {
            fileUploader.DeleteFile(previous);
        }

        return ResponseBase.Success<string>();
    }

    public static async Task<ResponseDto<string>> RemoveAsync(this IProductSellerRepo repo, FileUploader fileUploader, IdDto route, CancellationToken ct = default)
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