using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Server.Core.Commons;
using Server.Core.Modules.Cms.Models;

namespace Server.Core.Modules.Cms.Dto;

public class NewsFilter
{
    [DisplayName(Messages.Visible)]
    public bool? Visible { get; set; }

    [DisplayName(Messages.Category)]
    public int? CategoryId { get; set; }
}

public class NewsInput: News
{
    public new string? Image { get; set; }
    public new string? Thumbnail { get; set; }
    
    [DisplayName(Messages.Image)]
    [Required(ErrorMessage = Messages.RequiredModelState)]
    public IFormFile? ImageFile { get; set; }

    [DisplayName(Messages.Thumbnail)]
    [Required(ErrorMessage = Messages.RequiredModelState)]
    public IFormFile? ThumbnailFile { get; set; }
}

public class NewsInputUpdate : NewsInput
{
    
    [DisplayName(Messages.Image)]
    public IFormFile? ImageFile { get; set; }

    [DisplayName(Messages.Thumbnail)]
    public IFormFile? ThumbnailFile { get; set; }
}