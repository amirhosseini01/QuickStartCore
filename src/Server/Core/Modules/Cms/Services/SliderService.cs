using Server.Core.Commons;
using Server.Core.Commons.Datatables;
using Server.Core.Commons.UploadFile;
using Server.Core.Modules.Cms.Dto;
using Server.Core.Modules.Cms.Models;
using Server.Core.Modules.Cms.Repositories.Contracts;

namespace Server.Core.Modules.Cms.Services;

public static class SliderService
{
    public static async Task<DataTableResult<Slider>> GetDataTableAsync(this ISliderRepo repo, SliderFilter filter, DataTableFilter dataTableFilter, CancellationToken ct = default)
    {
        var query = repo.FilterQuery(filter: filter, dataTableFilter: dataTableFilter)
            .OrderByDescending(x => x.ViewOrder).ThenByDescending(x => x.Id);

        return await query.ToDataTableAsync(dataTableFilter, cancellationToken: ct);
    }

    public static async Task<Slider?> GetByIdAsync(this ISliderRepo repo, IdDto route, CancellationToken ct = default)
    {
        return await repo.FindAsync(id: route.Id, ct: ct);
    }

    public static async Task<ResponseDto<string>> AddAsync(this ISliderRepo repo, FileUploader fileUploader, SliderInput input, CancellationToken ct = default)
    {
        var imageUploadRes = await fileUploader.UploadFile(input.ImageFile);
        if (imageUploadRes.IsFailed)
        {
            return imageUploadRes;
        }
        
        input.Image = imageUploadRes.Obj;

        var thumbnailUploadRes = await fileUploader.UploadFile(input.ThumbnailFile);
        if (thumbnailUploadRes.IsFailed)
        {
            return thumbnailUploadRes;
        }
        input.Thumbnail = thumbnailUploadRes.Obj;

        await repo.AddAsync(input, ct);
        await repo.SaveChangesAsync(ct);

        return ResponseBase.Success<string>();
    }

    public static async Task<ResponseDto<string>> UpdateAsync(this ISliderRepo repo, FileUploader fileUploader, IdDto route, SliderInputUpdate input, CancellationToken ct = default)
    {
        var entity = await repo.GetByIdAsync(route, ct);
        if (entity is null)
        {
            return ResponseBase.Failed<string>(Messages.NotFound);
        }
        
        if (input.Image is not null)
        {
            var imageUploadRes = await fileUploader.UploadFile(input.ImageFile);
            entity.Image = imageUploadRes.Obj;
        }

        if (input.Thumbnail is not null)
        {
            var thumbnailUploadRes = await fileUploader.UploadFile(input.ThumbnailFile);
            entity.Thumbnail = thumbnailUploadRes.Obj;
        }

        new Mapper.SliderMapper().InputToEntity(input, entity);
        await repo.SaveChangesAsync(ct);
        return ResponseBase.Success<string>();
    }

    public static async Task<ResponseDto<string>> RemoveAsync(this ISliderRepo repo, FileUploader fileUploader, IdDto route, CancellationToken ct = default)
    {
        var entity = await repo.GetByIdAsync(route, ct);
        if (entity is null)
        {
            return ResponseBase.Failed<string>(Messages.NotFound);
        }
        
        fileUploader.DeleteFile(entity.Image);
        fileUploader.DeleteFile(entity.Thumbnail);
        repo.Remove(entity);
        await repo.SaveChangesAsync(ct);
        return ResponseBase.Success<string>();
    }
}
