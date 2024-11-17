using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Core.Commons;
using Server.Core.Commons.Datatables;
using Server.Core.Commons.UploadFile;
using Server.Core.Modules.Cms.Dto;
using Server.Core.Modules.Cms.Models;
using Server.Core.Modules.Cms.Repositories.Contracts;
using Server.Core.Modules.Cms.Services;

namespace Server.Areas.Admin.Pages.Cms;

public class NewsModel(INewsRepo repo, FileUploader fileUploader) : PageModel
{
    public NewsInputUpdate Input { get; set; }
    public NewsFilter Filter { get; set; }

    public void OnGet()
    {
    }

    public async Task<JsonResult> OnGetByIdAsync(IdDto routeVal, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
            if (ModelState.IsNotValid())
            {
                return ResponseBase.ReturnJsonInvalidData<News>(modelState: ModelState);
            }

        var entity = await repo.GetByIdAsync(route: routeVal, ct: ct);
        if (entity is null)
        {
            return ResponseBase.ReturnJsonNotFound<News>();
        }

        return ResponseBase.ReturnJsonSuccess(entity);
    }

    public async Task<JsonResult> OnPostListAsync(NewsFilter filter, DataTableFilter dataTableFilter, CancellationToken ct = default)
    {
        return ResponseBase.ReturnJson(await repo.GetDataTableAsync(filter: filter, dataTableFilter: dataTableFilter, ct: ct));
    }

    public async Task<JsonResult> OnPostAddAsync(NewsInput input, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
        {
            return ResponseBase.ReturnJson(ResponseBase.Failed<string>(ModelState.GetModeStateErrors()));
        }

        var addRes = await repo.AddAsync(fileUploader, input: input, ct: ct);
        return ResponseBase.ReturnJson(addRes);
    }

    public async Task<JsonResult> OnPostUpdateAsync(IdDto routeVal, NewsInputUpdate input, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
        {
            return ResponseBase.ReturnJson(ResponseBase.Failed<string>(ModelState.GetModeStateErrors()));
        }

        var editRes = await repo.UpdateAsync(fileUploader, route: routeVal, input: input, ct: ct);
        return ResponseBase.ReturnJson(editRes);
    }

    public async Task<JsonResult> OnGetRemoveAsync(IdDto routeVal, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
        {
            return ResponseBase.ReturnJson(ResponseBase.Failed<string>(ModelState.GetModeStateErrors()));
        }

        var res = await repo.RemoveAsync(fileUploader, routeVal, ct: ct);

        return ResponseBase.ReturnJson(res);
    }
}