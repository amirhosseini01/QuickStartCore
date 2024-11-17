using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Core.Commons;
using Server.Core.Commons.Datatables;
using Server.Core.Commons.UploadFile;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Models;
using Server.Core.Modules.Product.Repositories.Contracts;
using Server.Core.Modules.Product.Services;

namespace Server.Areas.Admin.Pages.Products;
public class IndexModel(IProductRepo repo, FileUploader fileUploader) : PageModel
{
    public required ProductInput Input { get; set; }
    public required ProductFilter Filter { get; set; }
    public void OnGet()
    {
    }

    public async Task<JsonResult> OnGetByIdAsync(IdDto routeVal, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
        {
            return ResponseBase.ReturnJsonInvalidData<Product>(modelState: ModelState);
        }

        var entity = await repo.GetByIdAsync(route: routeVal, ct: ct);
        if (entity is null)
        {
            return ResponseBase.ReturnJsonNotFound<Product>();
        }

        return ResponseBase.ReturnJsonSuccess(entity);
    }

    public async Task<JsonResult> OnPostListAsync(ProductFilter filter, DataTableFilter dataTableFilter, CancellationToken ct = default)
    {
        return ResponseBase.ReturnJson(await repo.GetDataTableAsync(filter: filter, dataTableFilter: dataTableFilter, ct: ct));
    }

    public async Task<JsonResult> OnPostAddAsync(ProductInput input, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
        {
            return ResponseBase.ReturnJson(ResponseBase.Failed<string>(ModelState.GetModeStateErrors()));
        }

        var addRes = await repo.AddAsync(input: input, fileUploader: fileUploader, ct: ct);
        return ResponseBase.ReturnJson(addRes);
    }

    public async Task<JsonResult> OnPostUpdateAsync(IdDto routeVal, ProductInput input, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
        {
            return ResponseBase.ReturnJson(ResponseBase.Failed<string>(ModelState.GetModeStateErrors()));
        }
        
        var editRes = await repo.UpdateAsync(fileUploader: fileUploader, routeVal, input: input, ct: ct);
        return ResponseBase.ReturnJson(editRes);
    }

    public async Task<JsonResult> OnGetRemoveAsync(IdDto routeVal, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
        {
            return ResponseBase.ReturnJson(ResponseBase.Failed<string>(ModelState.GetModeStateErrors()));
        }

        var res = await repo.RemoveAsync(fileUploader: fileUploader, routeVal, ct: ct);

        return ResponseBase.ReturnJson(res);
    }
}
