using Microsoft.AspNetCore.Mvc.Rendering;
using Server.Core.Commons;
using Server.Core.Commons.Datatables;
using Server.Core.Commons.UploadFile;
using Server.Core.Data.Models;
using Server.Core.Modules.Cms.Dto;
using Server.Core.Modules.Cms.Mapper;
using Server.Core.Modules.Cms.Models;
using Server.Core.Modules.Cms.Repositories.Contracts;

namespace Server.Core.Modules.Cms.Services;

public static class NewsService
{
    public static async Task<DataTableResult<News>> GetDataTableAsync(this INewsRepo repo, NewsFilter filter, DataTableFilter dataTableFilter, CancellationToken ct = default)
    {
        var query = repo.FilterQuery(filter: filter, dataTableFilter: dataTableFilter)
            .OrderByDescending(x => x.ViewOrder).ThenByDescending(x => x.Id);

        return await query.ToDataTableAsync(dataTableFilter, cancellationToken: ct);
    }

    public static async Task<PaginatedList<SelectListItem>> GetSelectListItemsAsync(this INewsRepo repo, NewsFilter filter, PaginatedListFilter paginatedListFilter, CancellationToken ct = default)
    {
        filter.Visible = true;
        var query = repo.FilterQuery(filter: filter)
            .OrderByDescending(x => x.ViewOrder).ThenByDescending(x => x.Id);

        return await PaginatedList<SelectListItem>.CreateAsync(
            source: query.MapEntityToSelectList(),
            paginatedListFilter: paginatedListFilter,
            ct: ct);
    }

    public static async Task<News?> GetByIdAsync(this INewsRepo repo, IdDto route, CancellationToken ct = default)
    {
        var filterQuery = new BaseQueryFilter()
        {
            Includes = [nameof(News.NewsCategory)]
        };
        return await repo.FirstOrDefaultAsync(predicate: x => x.Id == route.Id, filterQuery: filterQuery, ct: ct);
    }

    public static async Task<ResponseDto<string>> AddAsync(this INewsRepo repo, FileUploader fileUploader, NewsInput input, CancellationToken ct = default)
    {
        var entity = new Mapper.NewsMapper().InputToEntity(input);

        var uploadRes = await fileUploader.UploadFile(input.ImageFile);
        if (uploadRes.IsFailed)
        {
            return uploadRes;
        }

        entity.Image = uploadRes.Obj;

        uploadRes = await fileUploader.UploadFile(input.ThumbnailFile);
        if (uploadRes.IsFailed)
        {
            return uploadRes;
        }

        entity.Thumbnail = uploadRes.Obj;

        await repo.AddAsync(input, ct);
        await repo.SaveChangesAsync(ct);
        return ResponseBase.Success<string>();
    }

    public static async Task<ResponseDto<string>> UpdateAsync(this INewsRepo repo, FileUploader fileUploader, IdDto route, NewsInputUpdate input, CancellationToken ct = default)
    {
        // var entity = await repo.GetByIdAsync(route, ct);
        var entity = await repo.FindAsync(id: route.Id, ct: ct);
        if (entity is null)
        {
            return ResponseBase.Failed<string>(Messages.NotFound);
        }

        new NewsMapper().InputToEntity(input, entity);
        
        string? previousImage = null;
        if (input.ImageFile is not null)
        {
            var uploadRes = await fileUploader.UploadFile(input.ImageFile);
            if (uploadRes.IsFailed)
            {
                return uploadRes;
            }

            previousImage = entity.Image;
            entity.Image = uploadRes.Obj;
        }

        string? previousThumbnail = null;
        if (input.ThumbnailFile is not null)
        {
            var uploadRes = await fileUploader.UploadFile(input.ThumbnailFile);
            if (uploadRes.IsFailed)
            {
                return uploadRes;
            }

            previousThumbnail = entity.Thumbnail;
            entity.Thumbnail = uploadRes.Obj;
        }

        await repo.SaveChangesAsync(ct);
        if (previousImage is not null)
        {
            fileUploader.DeleteFile(previousImage);
        }

        if (previousThumbnail is not null)
        {
            fileUploader.DeleteFile(previousThumbnail);
        }

        return ResponseBase.Success<string>();
    }

    public static async Task<ResponseDto<string>> RemoveAsync(this INewsRepo repo, FileUploader fileUploader, IdDto route, CancellationToken ct = default)
    {
        var entity = await repo.GetByIdAsync(route, ct);
        if (entity is null)
        {
            return ResponseBase.Failed<string>(Messages.NotFound);
        }

        repo.Remove(entity);
        await repo.SaveChangesAsync(ct);
        fileUploader.DeleteFile(entity.Image);
        fileUploader.DeleteFile(entity.Thumbnail);
        return ResponseBase.Success<string>();
    }
}