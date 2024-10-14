using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Core.Commons;
using Server.Core.Data;

namespace Server.Core.Modules.Cms.Models;

public class News : BaseEntity
{
	[DisplayName(Messages.Category)]
	[Required(ErrorMessage = Messages.RequiredModelState)]
	[Range(minimum: ModelStatics.MinimumIdRange, maximum: ModelStatics.MaximumIdRange, ErrorMessage = Messages.RangeModelState)]
	public int CategoryId { get; set; }
	
	[DisplayName(Messages.ViewOrder)]
	[Range(minimum: ModelStatics.MinimumIdRange, maximum: ModelStatics.MaximumIdRange, ErrorMessage = Messages.RangeModelState)]
	public int? ViewOrder { get; set; }
	
	[DisplayName(Messages.Title)]
	[Required(ErrorMessage = Messages.RequiredModelState)]
	[MaxLength(ModelStatics.TitleRequiredLength, ErrorMessage = Messages.MaxLengthModelState)]
	[MinLength(ModelStatics.TitleMinimumLength, ErrorMessage = Messages.MinLengthModelState)]
	public string Title { get; set; }
	
	[DisplayName(Messages.Slug)]
	[Required(ErrorMessage = Messages.RequiredModelState)]
	[MaxLength(ModelStatics.TitleRequiredLength, ErrorMessage = Messages.MaxLengthModelState)]
	[MinLength(ModelStatics.TitleMinimumLength, ErrorMessage = Messages.MinLengthModelState)]
	public string Slug { get; set; }
	
	[DisplayName(Messages.Visible)]
	public bool Visible { get; set; } = true;
	
	[StringLength(ModelStatics.ImageRequiredLength)]
	public string Image { get; set; }

	[StringLength(ModelStatics.ImageRequiredLength)]
	public string Thumbnail { get; set; }
	
	[DisplayName(Messages.ShortDescription)]
	[Required(ErrorMessage = Messages.RequiredModelState)]
	[MaxLength(ModelStatics.DescriptionRequiredLength, ErrorMessage = Messages.MaxLengthModelState)]
	[MinLength(ModelStatics.DescriptionMinimumLength, ErrorMessage = Messages.MinLengthModelState)]
	public string ShortDescription { get; set; }
	
	[DisplayName(Messages.Description)]
	[Required(ErrorMessage = Messages.RequiredModelState)]
	[MinLength(ModelStatics.DescriptionMinimumLength, ErrorMessage = Messages.MinLengthModelState)]
	public string Description { get; set; }

	[ForeignKey(nameof(CategoryId))]
	public NewsCategory? NewsCategory { get; set; }
}
