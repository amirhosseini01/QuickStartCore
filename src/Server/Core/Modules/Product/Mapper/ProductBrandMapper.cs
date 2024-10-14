using Microsoft.AspNetCore.Mvc.Rendering;
using Riok.Mapperly.Abstractions;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Models;

namespace Server.Core.Modules.Product.Mapper;

[Mapper]
public static partial class ProductBrandMapperQuery
{
    public static partial IQueryable<SelectListItem> MapEntityToSelectList(this IQueryable<ProductBrand> q);
    
    [MapProperty(nameof(ProductBrand.Id), nameof(SelectListItem.Value))]
    [MapProperty(nameof(ProductBrand.Title), nameof(SelectListItem.Text))]
    public static partial SelectListItem EntityToSelectList(ProductBrand entity);
}

[Mapper]
public partial class ProductBrandMapper
{
    [MapperIgnoreSource(nameof(ProductBrand.Logo))]
    public partial ProductBrand InputToEntity(ProductBrandInput input);

    [MapperIgnoreSource(nameof(entity.Logo))]
    public partial void InputToEntity(ProductBrandInput input, ProductBrand entity);
}
