using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Core.Commons;
using Server.Core.Commons.Datatables;
using Server.Core.Commons.UploadFile;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Repositories.Contracts;
using Server.Core.Modules.Product.Services;

namespace Server.Areas.Admin.Pages.Products;
public class SellersModel(IProductSellerRepo repo, FileUploader fileUploader) : PageModel
{
    public ProductSellerInput Input { get; set; }
    public required ProductSellerFilter Filter { get; set; }
    public void OnGet()
    {
    }

    public async Task<JsonResult> OnGetByIdAsync(IdDto routeVal, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
        {
            return ResponseBase.ReturnJson(ResponseBase.Failed<string>(ModelState.GetModeStateErrors()));
        }

        var entity = await repo.GetByIdAsync(route: routeVal, ct: ct);
        if (entity is null)
        {
            return ResponseBase.ReturnJson(ResponseBase.Failed<string>(Messages.NotFound));
        }

        return ResponseBase.ReturnJson(ResponseBase.Success(entity));
    }

    public async Task<JsonResult> OnPostListAsync(ProductSellerFilter filter, DataTableFilter dataTableFilter, CancellationToken ct = default)
    {
        return ResponseBase.ReturnJson(await repo.GetDataTableAsync(filter, dataTableFilter, ct));
    }

    public async Task<JsonResult> OnPostAddAsync(ProductSellerInput input, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
        {
            return ResponseBase.ReturnJson(ResponseBase.Failed<string>(ModelState.GetModeStateErrors()));
        }

        var addRes = await repo.AddAsync(fileUploader, input: input, ct: ct);
        return ResponseBase.ReturnJson(addRes);
    }

    public async Task<JsonResult> OnPostUpdateAsync(IdDto routeVal, ProductSellerInput input, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
        {
            return ResponseBase.ReturnJson(ResponseBase.Failed<string>(ModelState.GetModeStateErrors()));
        }

        var editRes = await repo.UpdateAsync(fileUploader, routeVal, input: input, ct: ct);
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
