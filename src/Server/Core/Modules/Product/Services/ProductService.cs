using Microsoft.AspNetCore.Mvc.Rendering;
using Server.Core.Commons;
using Server.Core.Commons.Datatables;
using Server.Core.Commons.UploadFile;
using Server.Core.Data.Models;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Repositories.Contracts;

namespace Server.Core.Modules.Product.Services;

public static class ProductService
{
	// Admin
	public static async Task<DataTableResult<Models.Product>> GetDataTableAsync(this IProductRepo repo, ProductFilter filter, DataTableFilter dataTableFilter, CancellationToken ct = default)
	{
		var query = repo.FilterQuery(filter: filter, dataTableFilter: dataTableFilter)
			.OrderByDescending(x => x.ViewOrder).ThenByDescending(x => x.Id);

		return await query.ToDataTableAsync(dataTableFilter: dataTableFilter, cancellationToken: ct);
	}
	
	public static async Task<PaginatedList<SelectListItem>> GetSelectListItemsAsync(this IProductRepo repo, ProductFilter filter, PaginatedListFilter paginatedListFilter, CancellationToken ct = default)
	{
		var query = repo.FilterQuery(filter: filter).OrderByDescending(x => x.Id);

		return await PaginatedList<SelectListItem>.CreateAsync(source: Mapper.ProductMapperQuery.MapEntityToSelectList(query), paginatedListFilter: paginatedListFilter, ct: ct);
	}

	public static async Task<Models.Product?> GetByIdAsync(this IProductRepo repo, IdDto route, CancellationToken ct = default)
	{
		var filterQuery = new BaseQueryFilter
		{
			Includes = [nameof(Models.Product.ProductBrand), nameof(Models.Product.ProductCategory), nameof(Models.Product.ProductSeller)]
		};
		return await repo.FirstOrDefaultAsync(predicate: x => x.Id == route.Id, filterQuery: filterQuery, ct: ct);
	}

	public static async Task<ResponseDto<string>> AddAsync(this IProductRepo repo, FileUploader fileUploader, ProductInput input, CancellationToken ct = default)
	{
		var product = new Mapper.ProductMapper().InputToEntity(input);

		if (input.Image is not null)
		{
			var imageUploadRes = await fileUploader.UploadFile(input.ImageFile);
			if (imageUploadRes.IsFailed)
			{
				return imageUploadRes;
			}

			product.Image = imageUploadRes.Obj;
		}

		if (input.Thumbnail is not null)
		{
			var thumbnailUploadRes = await fileUploader.UploadFile(input.ThumbnailFile);
			if (thumbnailUploadRes.IsFailed)
			{
				return thumbnailUploadRes;
			}

			product.Thumbnail = thumbnailUploadRes.Obj;
		}

		await repo.AddAsync(product, ct);
		await repo.SaveChangesAsync(ct);
		return ResponseBase.Success<string>();
	}

	public static async Task<ResponseDto<string>> UpdateAsync(this IProductRepo repo, FileUploader fileUploader, IdDto route, ProductInput input, CancellationToken ct = default)
	{
		var entity = await repo.FindAsync(id: route.Id, ct: ct);
		if (entity is null)
		{
			return ResponseBase.Failed<string>(message: Messages.NotFound);
		}
		
		new Mapper.ProductMapper().InputToEntity(input, entity);
		string? previous = null;
		if (input.Image is not null)
		{
			var imageUploadRes = await fileUploader.UploadFile(input.ImageFile);
			if (imageUploadRes.IsFailed)
			{
				return imageUploadRes;
			}

			previous = entity.Image;
			entity.Image = imageUploadRes.Obj;
		}

		string? previousT = null;
		if (input.Thumbnail is not null)
		{
			var thumbnailUploadRes = await fileUploader.UploadFile(input.ThumbnailFile);
			if (thumbnailUploadRes.IsFailed)
			{
				return thumbnailUploadRes;
			}

			previousT = entity.Thumbnail;
			entity.Thumbnail = thumbnailUploadRes.Obj;
		}

		new Mapper.ProductMapper().InputToEntity(input, entity);
		await repo.SaveChangesAsync(ct);
		if (previous is not null)
		{
			fileUploader.DeleteFile(previous);
		}

		if (previousT is not null)
		{
			fileUploader.DeleteFile(previousT);
		}

		return ResponseBase.Success<string>();
	}

	public static async Task<ResponseDto<string>> RemoveAsync(this IProductRepo repo, FileUploader fileUploader, IdDto route, CancellationToken ct = default)
	{
		var entity = await repo.GetByIdAsync(route: route, ct: ct);
		if (entity is null)
		{
			return ResponseBase.Failed<string>(message: Messages.NotFound);
		}
		
		repo.Remove(entity);
		await repo.SaveChangesAsync(ct);
		if (entity.Image is not null)
		{
			fileUploader.DeleteFile(entity.Image);
		}

		if (entity.Thumbnail is not null)
		{
			fileUploader.DeleteFile(entity.Thumbnail);
		}
		
		return ResponseBase.Success<string>();
	}

	// user
	public static async Task<PaginatedList<Models.Product>> GetSpecialOffersAsync(this IProductRepo repo, PaginatedListFilter paginatedListFilter, CancellationToken ct = default)
	{
		// todo: use same product visibility filter for different methods
		var filter = new ProductFilter()
		{
			IsSpecialOffer = true,
			Saleable = true,
			Available = true
		};
		
		var query = repo.FilterQuery(filter: filter).OrderByDescending(x => x.ViewOrder).ThenByDescending(x => x.Id);

		return await PaginatedList<Models.Product>.CreateAsync(
			source: query,
			paginatedListFilter: paginatedListFilter,
			ct: ct);
	}

	public static async Task<PaginatedList<Models.Product>> GetByViewOrdersAsync(this IProductRepo repo, PaginatedListFilter paginatedListFilter, CancellationToken ct = default)
	{
		var filter = new ProductFilter()
		{
			IsSpecialOffer = false,
			Saleable = true,
			Available = true
		};
		
		var query = repo.FilterQuery(filter: filter).OrderByDescending(x => x.ViewOrder).ThenByDescending(x => x.Id);

		return await PaginatedList<Models.Product>.CreateAsync(
			source: query,
			paginatedListFilter: paginatedListFilter,
			ct: ct);
	}

	public static async Task<PaginatedList<Models.Product>> GetByTopDiscountsAsync(this IProductRepo repo, PaginatedListFilter paginatedListFilter, CancellationToken ct = default)
	{
		var filter = new ProductFilter()
		{
			IsSpecialOffer = false,
			HasDiscount = true,
			Saleable = true,
			Available = true
		};
		
		var query = repo.FilterQuery(filter: filter).OrderByDescending(x => x.ProductModels.Sum(xx => xx.Discount))
			.ThenByDescending(x => x.ViewOrder).ThenByDescending(x => x.Id);

		return await PaginatedList<Models.Product>.CreateAsync(
			source: query,
			paginatedListFilter: paginatedListFilter,
			ct: ct);
	}

	public static async Task<PaginatedList<Models.Product>> GetByTopSalesAsync(this IProductRepo repo, PaginatedListFilter paginatedListFilter, CancellationToken ct = default)
	{
		var filter = new ProductFilter()
		{
			IsSpecialOffer = false,
			Saleable = true,
			Available = true
		};
		
		var query = repo.FilterQuery(filter: filter).OrderByDescending(x => x.ProductStocks.Sum(xx => xx.Value))
			.ThenByDescending(x => x.ViewOrder).ThenByDescending(x => x.Id);

		return await PaginatedList<Models.Product>.CreateAsync(
			source: query,
			paginatedListFilter: paginatedListFilter,
			ct: ct);
	}

	public static async Task<PaginatedList<Models.Product>> GetByTopViewedAsync(this IProductRepo repo, PaginatedListFilter paginatedListFilter, CancellationToken ct = default)
	{
		var filter = new ProductFilter()
		{
			IsSpecialOffer = false,
			Saleable = true,
			Available = true
		};
		
		var query = repo.FilterQuery(filter: filter).OrderByDescending(x => x.ViewCount)
			.ThenByDescending(x => x.ViewOrder).ThenByDescending(x => x.Id);

		return await PaginatedList<Models.Product>.CreateAsync(
			source: query,
			paginatedListFilter: paginatedListFilter,
			ct: ct);
	}
}
