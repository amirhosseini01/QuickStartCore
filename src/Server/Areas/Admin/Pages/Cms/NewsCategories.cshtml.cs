using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Core.Commons;
using Server.Core.Commons.Datatables;
using Server.Core.Modules.Cms.Dto;
using Server.Core.Modules.Cms.Models;
using Server.Core.Modules.Cms.Repositories.Contracts;
using Server.Core.Modules.Cms.Repositories.Implementations;
using Server.Core.Modules.Cms.Services;

namespace Server.Areas.Admin.Pages.Cms;

public class NewsCategoriesModel(INewsCategoryRepo repo) : PageModel
{
    public NewsCategory Input { get; set; }
    public NewsCategoryFilter Filter { get; set; }

    public void OnGet()
    {
    }

    public async Task<JsonResult> OnGetByIdAsync(IdDto routeVal, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
        {
            return ResponseBase.ReturnJsonInvalidData<NewsCategory>(modelState: ModelState);
        }

        var entity = await repo.GetByIdAsync(route: routeVal, ct: ct);
        if (entity is null)
        {
            return ResponseBase.ReturnJsonNotFound<NewsCategory>();
        }

        return ResponseBase.ReturnJsonSuccess(entity);
    }

    public async Task<JsonResult> OnPostListAsync(NewsCategoryFilter filter, DataTableFilter dataTableFilter, CancellationToken ct = default)
    {
        return ResponseBase.ReturnJson(await repo.GetDataTableAsync(filter: filter, dataTableFilter: dataTableFilter, ct: ct));
    }

    public async Task<JsonResult> OnPostAddAsync(NewsCategory input, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
        {
            return ResponseBase.ReturnJson(ResponseBase.Failed<string>(ModelState.GetModeStateErrors()));
        }

        var addRes = await repo.AddAsync(input: input, ct: ct);
        return ResponseBase.ReturnJson(addRes);
    }

    public async Task<JsonResult> OnPostUpdateAsync(IdDto routeVal, NewsCategory input, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
        {
            return ResponseBase.ReturnJson(ResponseBase.Failed<string>(ModelState.GetModeStateErrors()));
        }

        var editRes = await repo.UpdateAsync(route: routeVal, input: input, ct: ct);
        return ResponseBase.ReturnJson(editRes);
    }

    public async Task<JsonResult> OnGetRemoveAsync(IdDto routeVal, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
        {
            return ResponseBase.ReturnJson(ResponseBase.Failed<string>(ModelState.GetModeStateErrors()));
        }

        var res = await repo.RemoveAsync(routeVal, ct);

        return ResponseBase.ReturnJson(ResponseBase.Success<string>());
    }
}