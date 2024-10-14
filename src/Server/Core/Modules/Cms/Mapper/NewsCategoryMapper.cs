using Microsoft.AspNetCore.Mvc.Rendering;
using Riok.Mapperly.Abstractions;
using Server.Core.Modules.Cms.Models;

namespace Server.Core.Modules.Cms.Mapper;

[Mapper]
public static partial class NewsCategoryMapperQuery
{
    public static partial IQueryable<SelectListItem> MapEntityToSelectList(this IQueryable<NewsCategory> q);
    
    [MapProperty(nameof(NewsCategory.Id), nameof(SelectListItem.Value))]
    [MapProperty(nameof(NewsCategory.Title), nameof(SelectListItem.Text))]
    public static partial SelectListItem EntityToSelectList(NewsCategory entity);
}

[Mapper]
public partial class NewsCategoryMapper
{
    public partial void InputToEntity(NewsCategory input, NewsCategory entity);
}