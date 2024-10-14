using Microsoft.EntityFrameworkCore;
using Server.Core.Commons;
using Server.Core.Commons.Datatables;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Models;
using Server.Core.Modules.Product.Repositories.Contracts;

namespace Server.Core.Modules.Product.Services;

public static class ProductStockService
{
    public static async Task<DataTableResult<ProductStock>> GetDataTableAsync(this IProductStockRepo repo, ProductStockFilter filter, DataTableFilter dataTableFilter, CancellationToken ct = default)
    {
        var query = repo.FilterQuery(filter: filter, dataTableFilter: dataTableFilter)
            .Include(x=> x.Product)
            .Include(x=> x.ProductModel)
            .OrderByDescending(x => x.Id);

        return await query.ToDataTableAsync(dataTableFilter, ct);
    }

    public static async Task<ResponseDto<string>> AddAsync(this IProductStockRepo repo, ProductStock input, CancellationToken ct = default)
    {
        var productStockFilter = new ProductStockFilter
        {
            ProductModelId = input.ProductModelId,
            ProductId = input.ProductId
        };

        var stockBalance = await repo.FilterQuery(productStockFilter).SumAsync(x => x.Value, ct);
            
        // avoid stock balance became negative
        if (input.Value < 0 && stockBalance - input.Value < 0)
        {
            return ResponseBase.Failed<string>(Messages.StockCouldNotBecomeNegative);
        }
        
        await repo.AddAsync(input, ct);
        await repo.SaveChangesAsync(ct);
        return ResponseBase.Success<string>();
    }
}
