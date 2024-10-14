using Riok.Mapperly.Abstractions;
using Server.Core.Modules.Cms.Dto;
using Server.Core.Modules.Cms.Models;

namespace Server.Core.Modules.Cms.Mapper;

[Mapper]
public partial class SliderMapper
{
    [MapperIgnoreSource(nameof(entity.Image))]
    [MapperIgnoreSource(nameof(entity.Thumbnail))]
    public partial void InputToEntity(SliderInputUpdate input, Slider entity);
}
