using Microsoft.AspNetCore.Mvc.Rendering;
using Riok.Mapperly.Abstractions;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Models;

namespace Server.Core.Modules.Product.Mapper;

[Mapper]
public static partial class ProductSellerMapperQuery
{
    public static partial IQueryable<SelectListItem> MapEntityToSelectList(this IQueryable<ProductSeller> q);
    
    [MapProperty(nameof(ProductSeller.Id), nameof(SelectListItem.Value))]
    [MapProperty(nameof(ProductSeller.Title), nameof(SelectListItem.Text))]
    public static partial SelectListItem EntityToSelectList(ProductSeller entity);
}

[Mapper]
public partial class ProductSellerMapper
{
    public partial ProductSeller InputToEntity(ProductSellerInput input);

    [MapperIgnoreSource(nameof(entity.Logo))]
    public partial void InputToEntity(ProductSellerInput input, ProductSeller entity);
}
