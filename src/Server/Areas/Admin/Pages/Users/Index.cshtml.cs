using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Core.Commons;
using Server.Core.Commons.Datatables;
using Server.Core.Modules.User.Dto;
using Server.Core.Modules.User.Models;
using Server.Core.Modules.User.Repositories.Contracts;
using Server.Core.Modules.User.Services;

namespace Server.Areas.Admin.Pages.Users;
public class IndexModel(IUserRepo repo) : PageModel
{
	public UserInputUpdate Input { get; set; }
    public UserFilter Filter { get; set; }
	public async Task<JsonResult> OnGetByIdAsync(IdDto routeVal, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
        {
            return ResponseBase.ReturnJsonInvalidData<AppUser>(modelState: ModelState);
        }

        var entity = await repo.GetByIdAsync(route: routeVal, ct: ct);
        if (entity is null)
        {
            return ResponseBase.ReturnJsonNotFound<AppUser>();
        }

        return ResponseBase.ReturnJsonSuccess(entity);
    }

    public async Task<JsonResult> OnPostListAsync(UserFilter filter, DataTableFilter dataTableFilter, CancellationToken ct = default)
    {
        return ResponseBase.ReturnJson(await repo.GetDataTableAsync(filter: filter, dataTableFilter: dataTableFilter, ct));
    }

    public async Task<JsonResult> OnPostAddAsync(UserInput input, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
        {
            return ResponseBase.ReturnJson(ResponseBase.Failed<string>(ModelState.GetModeStateErrors()));
        }

        var addRes = await repo.AddAsync(input: input, ct: ct);
        return ResponseBase.ReturnJson(addRes);
    }

    public async Task<JsonResult> OnPostUpdateAsync(IdDto routeVal, UserInputUpdate input, CancellationToken ct = default)
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

        var res = await repo.RemoveAsync(route: routeVal, ct: ct);

        return ResponseBase.ReturnJson(res);
    }
}
