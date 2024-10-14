using Microsoft.AspNetCore.Mvc.Rendering;
using Riok.Mapperly.Abstractions;
using Server.Core.Modules.Product.Models;

namespace Server.Core.Modules.Product.Mapper;

[Mapper]
public static partial class ProductModelMapperQuery
{
    public static partial IQueryable<SelectListItem> MapEntityToSelectList(this IQueryable<ProductModel> q);
	
    [MapProperty(nameof(ProductModel.Id), nameof(SelectListItem.Value))]
    [MapProperty(nameof(ProductModel.Title), nameof(SelectListItem.Text))]
    public static partial SelectListItem EntityToSelectList(ProductModel entity);
}

[Mapper]
public partial class ProductModelMapper
{
    public partial void InputToEntity(ProductModel input, ProductModel entity);
}
