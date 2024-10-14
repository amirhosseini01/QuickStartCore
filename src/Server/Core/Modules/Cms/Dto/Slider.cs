using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Server.Core.Commons;
using Server.Core.Modules.Cms.Models;

namespace Server.Core.Modules.Cms.Dto;

public class SliderFilter
{
    [DisplayName(Messages.Visible)]
    public bool? Visible { get; internal set; }
}

public class SliderInput: Slider
{
    [Required(ErrorMessage = Messages.RequiredModelState)]
    public IFormFile ImageFile { get; set; }

    [Required(ErrorMessage = Messages.RequiredModelState)]
    public IFormFile ThumbnailFile { get; set; }
}

public class SliderInputUpdate : Slider
{
    public new IFormFile? ImageFile { get; set; }
    public new IFormFile? ThumbnailFile { get; set; }
}
