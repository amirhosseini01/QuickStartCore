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
public class NewsCategoriesModel(INewsCategoryRepo service) : PageModel
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
            return ResponseBase.ReturnJson(ResponseBase.Failed<string>(ModelState.GetModeStateErrors()));
        }

        var entity = await service.GetByIdAsync(route: routeVal, ct: ct);
        if (entity is null)
        {
            return ResponseBase.ReturnJson(ResponseBase.Failed<string>(Messages.NotFound));
        }

        return ResponseBase.ReturnJson(ResponseBase.Success(entity));
    }

    public async Task<JsonResult> OnPostListAsync(NewsCategoryFilter filter, DataTableFilter dataTableFilter, CancellationToken ct = default)
    {
        return ResponseBase.ReturnJson(await service.GetDataTableAsync(filter: filter, dataTableFilter: dataTableFilter, ct: ct));
    }

    public async Task<JsonResult> OnPostAddAsync(NewsCategory input, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
        {
            return ResponseBase.ReturnJson(ResponseBase.Failed<string>(ModelState.GetModeStateErrors()));
        }

        var addRes = await service.AddAsync(input: input, ct: ct);
        return ResponseBase.ReturnJson(addRes);
    }

    public async Task<JsonResult> OnPostUpdateAsync(IdDto routeVal, NewsCategory input, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
        {
            return ResponseBase.ReturnJson(ResponseBase.Failed<string>(ModelState.GetModeStateErrors()));
        }

        var editRes = await service.UpdateAsync(route: routeVal, input: input, ct: ct);
        return ResponseBase.ReturnJson(editRes);
    }

    public async Task<JsonResult> OnGetRemoveAsync(IdDto routeVal, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
        {
            return ResponseBase.ReturnJson(ResponseBase.Failed<string>(ModelState.GetModeStateErrors()));
        }

        var res = await service.RemoveAsync(routeVal, ct);

        return ResponseBase.ReturnJson(ResponseBase.Success<string>());
    }
}
