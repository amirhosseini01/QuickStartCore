using Microsoft.AspNetCore.Mvc.Rendering;
using Riok.Mapperly.Abstractions;
using Server.Core.Modules.Product.Dto;
using Server.Core.Modules.Product.Models;

namespace Server.Core.Modules.Product.Mapper;

[Mapper]
public static partial class ProductCategoryMapperQuery
{
    public static partial IQueryable<SelectListItem> MapEntityToSelectList(this IQueryable<ProductCategory> q);
    
    [MapProperty(nameof(ProductCategory.Id), nameof(SelectListItem.Value))]
    [MapProperty(nameof(ProductCategory.Title), nameof(SelectListItem.Text))]
    public static partial SelectListItem EntityToSelectList(ProductCategory entity);
}

[Mapper]
public partial class ProductCategoryMapper
{
    [MapperIgnoreSource(nameof(ProductCategory.Image))]
    public partial ProductCategory InputToEntity(ProductCategoryInput input);

    [MapperIgnoreSource(nameof(ProductCategory.Image))]
    public partial void InputToEntity(ProductCategoryInput input, ProductCategory entity);
}
