using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Server.Core.Commons;
using Server.Core.Modules.Cms.Dto;
using Server.Core.Modules.Cms.Repositories.Contracts;
using Server.Core.Modules.Cms.Services;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Repositories.Contracts;
using Server.Core.Modules.Product.Services;
using Server.Core.Modules.User.Dto;
using Server.Core.Modules.User.Repositories.Contracts;
using Server.Core.Modules.User.Services;

namespace Server.Areas.Admin.Pages;

public class SelectListModel(
	IUserRepo userRepo,
	IProductCategoryRepo productCategoryRepo,
	IProductSellerRepo productSellerRepo,
	IProductModelRepo productModelRepo,
	INewsCategoryRepo newsCategoryRepo,
	IProductRepo productRepo,
	IProductBrandRepo productBrandRepo
	) : PageModel
{
    public async Task<JsonResult> OnGetBrands(ProductBrandFilter filter, PaginatedListFilter paginatedListFilter, CancellationToken ct = default)
    {
        var result = await productBrandRepo.GetSelectListItemsAsync(filter: filter, paginatedListFilter: paginatedListFilter, ct: ct);
        return ResponseBase.ReturnJson(new Select2Results()
        {
            HasNextPage = result.HasNextPage,
            Items = result
        });
    }
    
    public async Task<JsonResult> OnGetCategories(ProductCategoryFilter filter, PaginatedListFilter paginatedListFilter, CancellationToken ct = default)
    {
	    var result = await productCategoryRepo.GetSelectListItemsAsync(filter, paginatedListFilter, ct: ct);
	    return ResponseBase.ReturnJson(new Select2Results()
	    {
		    HasNextPage = result.HasNextPage,
		    Items = result
	    });
    }
    
    public async Task<JsonResult> OnGetSellers(ProductSellerFilter filter, PaginatedListFilter paginatedListFilter, CancellationToken ct = default)
    {
	    var result = await productSellerRepo.GetSelectListItemsAsync(filter: filter, paginatedListFilter: paginatedListFilter, ct: ct);
	    return ResponseBase.ReturnJson(new Select2Results()
	    {
		    HasNextPage = result.HasNextPage,
		    Items = result
	    });
    }
    
    public async Task<JsonResult> OnGetUsers(UserFilter filter, PaginatedListFilter paginatedListFilter, CancellationToken ct = default)
    {
	    var result = await userRepo.GetSelectListItemsAsync(filter: filter, paginatedListFilter: paginatedListFilter, ct: ct);
	    return ResponseBase.ReturnJson(new Select2Results()
	    {
		    HasNextPage = result.HasNextPage,
		    Items = result
	    });
    }
    
    public async Task<JsonResult> OnGetProducts(ProductFilter filter, PaginatedListFilter paginatedListFilter, CancellationToken ct = default)
    {
	    var result = await productRepo.GetSelectListItemsAsync(filter: filter, paginatedListFilter: paginatedListFilter, ct: ct);
	    return ResponseBase.ReturnJson(new Select2Results()
	    {
		    HasNextPage = result.HasNextPage,
		    Items = result
	    });
    }
    
    public async Task<JsonResult> OnGetProductModels(ProductModelFilter filter, PaginatedListFilter paginatedListFilter, CancellationToken ct = default)
    {
	    var result = await productModelRepo.GetSelectListItemsAsync(filter, paginatedListFilter: paginatedListFilter, ct: ct);
	    return ResponseBase.ReturnJson(new Select2Results()
	    {
		    HasNextPage = result.HasNextPage,
		    Items = result
	    });
    }
    
    public async Task<JsonResult> OnGetNewsCategories(NewsCategoryFilter filter, PaginatedListFilter paginatedListFilter, CancellationToken ct = default)
    {
	    var result = await newsCategoryRepo.GetSelectListItemsAsync(filter: filter, paginatedListFilter: paginatedListFilter, ct: ct);
	    return ResponseBase.ReturnJson(new Select2Results()
	    {
		    HasNextPage = result.HasNextPage,
		    Items = result
	    });
    }
}
