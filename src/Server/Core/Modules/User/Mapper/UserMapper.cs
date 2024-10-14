using Microsoft.AspNetCore.Mvc.Rendering;
using Riok.Mapperly.Abstractions;
using Server.Core.Modules.User.Dto;
using Server.Core.Modules.User.Models;

namespace Server.Core.Modules.User.Mapper;

[Mapper]
public static partial class UserMapperQuery
{
    public static partial IQueryable<SelectListItem> MapEntityToSelectList(this IQueryable<AppUser> q);

    [MapProperty(nameof(AppUser.Id), nameof(SelectListItem.Value))]
    [MapProperty(nameof(AppUser.UserName), nameof(SelectListItem.Text))]
    private static partial SelectListItem AppUserToSelectList(AppUser entity);
}

[Mapper]
public partial class UserMapper
{
	public partial AppUser InputToEntity(UserInput inputDto);
	
	public partial void InputToEntity(UserInput inputDto, AppUser entity);
}
