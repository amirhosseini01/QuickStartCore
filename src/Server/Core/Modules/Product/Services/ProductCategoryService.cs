using Microsoft.AspNetCore.Mvc.Rendering;
using Server.Core.Commons;
using Server.Core.Commons.Datatables;
using Server.Core.Commons.UploadFile;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Models;
using Server.Core.Modules.Product.Repositories.Contracts;

namespace Server.Core.Modules.Product.Services;

public static class ProductCategoryService
{
    public static async Task<DataTableResult<ProductCategory>> GetDataTableAsync(this IProductCategoryRepo repo, ProductCategoryFilter filter, DataTableFilter dataTableFilter, CancellationToken ct = default)
    {
        var query = repo.FilterQuery(filter: filter, dataTableFilter: dataTableFilter)
            .OrderByDescending(x => x.ViewOrder).ThenByDescending(x => x.Id);

        return await query.ToDataTableAsync(dataTableFilter, ct);
    }

    public static async Task<PaginatedList<SelectListItem>> GetSelectListItemsAsync(this IProductCategoryRepo repo, ProductCategoryFilter filter, PaginatedListFilter paginatedListFilter, CancellationToken ct = default)
    {
        var query = repo.FilterQuery(filter)
            .OrderByDescending(x => x.ViewOrder).ThenByDescending(x => x.Id);

        return await PaginatedList<SelectListItem>.CreateAsync(
            source: Mapper.ProductCategoryMapperQuery.MapEntityToSelectList(query),
            paginatedListFilter,
            ct: ct);
    }
    public static async Task<ProductCategory?> GetByIdAsync(this IProductCategoryRepo repo, IdDto route, CancellationToken ct)
    {
        return await repo.FindAsync(route.Id, ct);
    }

    public static async Task<ResponseDto<string>> AddAsync(this IProductCategoryRepo repo, FileUploader fileUploader, ProductCategoryInput input, CancellationToken ct = default)
    {
        var entity = new Mapper.ProductCategoryMapper().InputToEntity(input);

        if (input.ImageFile is not null)
        {
            var uploadRes = await fileUploader.UploadFile(input.ImageFile);
            if (uploadRes.IsFailed)
            {
                return uploadRes;
            }
            entity.Image = uploadRes.Obj;
        }

        await repo.AddAsync(entity, ct);
        await repo.SaveChangesAsync(ct);

        return ResponseBase.Success<string>();
    }

    public static async Task<ResponseDto<string>> UpdateAsync(this IProductCategoryRepo repo, FileUploader fileUploader, IdDto route, ProductCategoryInput input, CancellationToken ct = default)
    {
        var entity = await repo.GetByIdAsync(route, ct);
        if (entity is null)
        {
            return ResponseBase.Failed<string>(Messages.NotFound);
        }
        
        new Mapper.ProductCategoryMapper().InputToEntity(input, entity);
        string? previous = null;
        if (input.ImageFile is not null)
        {
            var uploadRes = await fileUploader.UploadFile(input.ImageFile);
            if (uploadRes.IsFailed)
            {
                return uploadRes;
            }
            previous = entity.Image;
            entity.Image = uploadRes.Obj;

        }

        await repo.SaveChangesAsync(ct);
        if (previous is not null)
        {
            fileUploader.DeleteFile(previous);
        }
        return ResponseBase.Success<string>();
    }

    public static async Task<ResponseDto<string>> RemoveAsync(this IProductCategoryRepo repo, FileUploader fileUploader, IdDto route, CancellationToken ct = default)
    {
        var entity = await repo.GetByIdAsync(route: route, ct: ct);
        if (entity is null)
        {
            return ResponseBase.Failed<string>(message: Messages.NotFound);
        }
        
        repo.Remove(entity);
        await repo.SaveChangesAsync(ct);
        if (entity.Image is not null)
        {
            fileUploader.DeleteFile(entity.Image);
        }

        return ResponseBase.Success<string>();
    }
}
