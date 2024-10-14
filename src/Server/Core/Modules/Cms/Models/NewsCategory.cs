using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Core.Commons;
using Server.Core.Data;

namespace Server.Core.Modules.Cms.Models;

public class NewsCategory : BaseEntity
{
	public int? ParentId { get; set; }
	
	[DisplayName(Messages.ViewOrder)]
	[Range(minimum: ModelStatics.MinimumIdRange, maximum: ModelStatics.MaximumIdRange, ErrorMessage = Messages.RangeModelState)]
	public int? ViewOrder { get; set; }
	
	[DisplayName(Messages.Title)]
	[Required(ErrorMessage = Messages.RequiredModelState)]
	[MaxLength(ModelStatics.TitleRequiredLength, ErrorMessage = Messages.MaxLengthModelState)]
	[MinLength(ModelStatics.TitleMinimumLength, ErrorMessage = Messages.MinLengthModelState)]
	public string Title { get; set; }
	
	[DisplayName(Messages.Visible)]
	public bool Visible { get; set; } = true;

	[InverseProperty(nameof(News.NewsCategory))]
	public virtual ICollection<News>? NewsCollection { get; set; }
}
