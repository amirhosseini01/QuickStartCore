using Microsoft.AspNetCore.Mvc.Rendering;
using Riok.Mapperly.Abstractions;
using Server.Core.Modules.Product.Dto;

namespace Server.Core.Modules.Product.Mapper;

[Mapper]
public static partial class ProductMapperQuery
{
	public static partial IQueryable<SelectListItem> MapEntityToSelectList(this IQueryable<Models.Product> q);

	[MapProperty(nameof(Models.Product.Id), nameof(SelectListItem.Value))]
	[MapProperty(nameof(Models.Product.Title), nameof(SelectListItem.Text))]
	public static partial SelectListItem EntityToSelectList(Models.Product entity);
}

[Mapper]
public partial class ProductMapper
{
	[MapperIgnoreSource(nameof(Models.Product.Image))]
	[MapperIgnoreSource(nameof(Models.Product.Thumbnail))]
	public partial Models.Product InputToEntity(ProductInput input);

	[MapperIgnoreSource(nameof(Models.Product.Image))]
	[MapperIgnoreSource(nameof(Models.Product.Thumbnail))]
	public partial void InputToEntity(ProductInput input, Models.Product entity);
}
