using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Core.Commons;
using Server.Core.Commons.Datatables;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Models;
using Server.Core.Modules.Product.Repositories.Contracts;
using Server.Core.Modules.Product.Services;

namespace Server.Areas.Admin.Pages.Products;
public class ModelsModel(IProductModelRepo repo) : PageModel
{
    public ProductModel Input { get; set; }
    public ProductModelFilter Filter { get; set; }
    public void OnGet()
    {
    }

    public async Task<JsonResult> OnGetByIdAsync(IdDto routeVal, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
        {
            return ResponseBase.ReturnJsonInvalidData<ProductModel>(modelState: ModelState);
        }

        var entity = await repo.GetByIdAsync(route: routeVal, ct: ct);
        if (entity is null)
        {
            return ResponseBase.ReturnJsonNotFound<ProductModel>();
        }

        return ResponseBase.ReturnJsonSuccess(entity);
    }

    public async Task<JsonResult> OnPostListAsync(ProductModelFilter filter, DataTableFilter dataTableFilter, CancellationToken ct = default)
    {
        return ResponseBase.ReturnJson(await repo.GetDataTableAsync(filter, dataTableFilter, ct));
    }

    public async Task<JsonResult> OnPostAddAsync(ProductModel input, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
        {
            return ResponseBase.ReturnJson(ResponseBase.Failed<string>(ModelState.GetModeStateErrors()));
        }

        var addRes = await repo.AddAsync(input: input, ct: ct);
        return ResponseBase.ReturnJson(addRes);
    }

    public async Task<JsonResult> OnPostUpdateAsync(IdDto routeVal, ProductModel input, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
        {
            return ResponseBase.ReturnJson(ResponseBase.Failed<string>(ModelState.GetModeStateErrors()));
        }

        var editRes = await repo.UpdateAsync(routeVal, input: input, ct: ct);
        return ResponseBase.ReturnJson(editRes);
    }

    public async Task<JsonResult> OnGetRemoveAsync(IdDto routeVal, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
        {
            return ResponseBase.ReturnJson(ResponseBase.Failed<string>(ModelState.GetModeStateErrors()));
        }

        var res = await repo.RemoveAsync(routeVal, ct: ct);

        return ResponseBase.ReturnJson(res);
    }
}
