using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Core.Commons;
using Server.Core.Commons.Datatables;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Models;
using Server.Core.Modules.Product.Repositories.Contracts;
using Server.Core.Modules.Product.Services;

namespace Server.Areas.Admin.Pages.Products;

public class StocksModel(IProductStockRepo repo) : PageModel
{
	public ProductStock Input { get; set; }
	public required ProductStockFilter Filter { get; set; }

	public void OnGet()
	{
	}
	
	public async Task<JsonResult> OnPostListAsync(ProductStockFilter filter, DataTableFilter dataTableFilter, CancellationToken ct = default)
	{
		return ResponseBase.ReturnJson(await repo.GetDataTableAsync(filter: filter, dataTableFilter: dataTableFilter, ct: ct));
	}

	public async Task<JsonResult> OnPostAddAsync(ProductStock input, CancellationToken ct = default)
	{
		if (ModelState.IsNotValid())
		{
			return ResponseBase.ReturnJson(ResponseBase.Failed<string>(ModelState.GetModeStateErrors()));
		}

		var addRes = await repo.AddAsync(input: input, ct: ct);
		return ResponseBase.ReturnJson(addRes);
	}
}
