using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Core.Commons;
using Server.Core.Commons.Datatables;
using Server.Core.Commons.UploadFile;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Repositories.Contracts;
using Server.Core.Modules.Product.Services;

namespace Server.Areas.Admin.Pages.Products;
public class BrandsModel(IProductBrandRepo repo, FileUploader fileUploader) : PageModel
{
    public ProductBrandInput Input { get; set; }
    public ProductBrandFilter Filter { get; set; }
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

    public async Task<JsonResult> OnPostListAsync(ProductBrandFilter filter, DataTableFilter dataTableFilter, CancellationToken ct = default)
    {
        var entities = await repo.GetDataTableAsync(filter: filter, dataTableFilter: dataTableFilter, ct: ct);
        return ResponseBase.ReturnJson(entities);
    }

    public async Task<JsonResult> OnPostAddAsync(ProductBrandInput input, CancellationToken ct = default)
    {
        if (ModelState.IsNotValid())
        {
            return ResponseBase.ReturnJson(ResponseBase.Failed<string>(ModelState.GetModeStateErrors()));
        }

        var addRes = await repo.AddAsync(fileUploader: fileUploader, input: input, ct: ct);
        return ResponseBase.ReturnJson(addRes);
    }

    public async Task<JsonResult> OnPostUpdateAsync(IdDto routeVal, ProductBrandInput input, CancellationToken ct = default)
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

        var res = await repo.RemoveAsync(fileUploader: fileUploader, route: routeVal, ct: ct);

        return ResponseBase.ReturnJson(res);
    }
}
