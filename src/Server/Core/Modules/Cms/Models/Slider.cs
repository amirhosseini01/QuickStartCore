using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Server.Core.Commons;
using Server.Core.Data;
using Server.Core.Modules.Cms.Enums;

namespace Server.Core.Modules.Cms.Models;
public class Slider : BaseEntity
{
    [DisplayName(Messages.ViewOrder)]
    [Range(minimum: ModelStatics.MinimumIdRange, maximum: ModelStatics.MaximumIdRange, ErrorMessage = Messages.RangeModelState)]
    public int? ViewOrder { get; set; }

    [DisplayName(Messages.Title)]
    [Required(ErrorMessage = Messages.RequiredModelState)]
    [MaxLength(ModelStatics.TitleRequiredLength, ErrorMessage = Messages.MaxLengthModelState)]
    [MinLength(ModelStatics.TitleMinimumLength, ErrorMessage = Messages.MinLengthModelState)]
    public string Title { get; set; }

    [Required(ErrorMessage = Messages.RequiredModelState)]
    [Column(TypeName = ModelStatics.Nvarchar50)]
    [Newtonsoft.Json.JsonConverter(typeof(JsonStringEnumConverter))]
    public SliderPlace SliderPlace { get; set; }

    [DisplayName(Messages.Visible)]
    public bool Visible { get; set; } = true;

    [DisplayName(Messages.Image)]
    [StringLength(ModelStatics.ImageRequiredLength)]
    public string Image { get; set; }

    [DisplayName(Messages.Thumbnail)]
    [StringLength(ModelStatics.ImageRequiredLength)]
    public string Thumbnail { get; set; }

    [DisplayName(Messages.Link)]
    [StringLength(maximumLength: ModelStatics.UrlRequiredLength, MinimumLength = ModelStatics.UrlMinimumLength)]
    public string? Link { get; set; }
}
