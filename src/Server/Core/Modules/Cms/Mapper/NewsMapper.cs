using Microsoft.AspNetCore.Mvc.Rendering;
using Riok.Mapperly.Abstractions;
using Server.Core.Modules.Cms.Dto;
using Server.Core.Modules.Cms.Models;

namespace Server.Core.Modules.Cms.Mapper;

[Mapper]
public static partial class NewsMapperQuery
{
    public static partial IQueryable<SelectListItem> MapEntityToSelectList(this IQueryable<News> q);
    
    [MapProperty(nameof(News.Id), nameof(SelectListItem.Value))]
    [MapProperty(nameof(News.Title), nameof(SelectListItem.Text))]
    public static partial SelectListItem EntityToSelectList(News entity);
}

[Mapper]
public partial class NewsMapper
{
    [MapperIgnoreSource(nameof(News.Image))]
    [MapperIgnoreSource(nameof(News.Thumbnail))]
    public partial News InputToEntity(NewsInput input);

    [MapperIgnoreSource(nameof(News.Image))]
    [MapperIgnoreSource(nameof(News.Thumbnail))]
    public partial void InputToEntity(NewsInputUpdate input, News entity);
}