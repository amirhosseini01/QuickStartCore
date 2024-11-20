using System.ComponentModel;
using Server.Core.Commons;

namespace Server.Core.Modules.Product.Dto;

// Admin
public class ProductFilter
{
	[DisplayName(Messages.Visible)] public bool? Visible { get; set; } = true;
	
	[DisplayName(Messages.Saleable)]
	public bool? Saleable { get; set; }
	
	[DisplayName(Messages.IsSpecialOffer)]
	public bool? IsSpecialOffer { get; set; }
	
	[DisplayName(Messages.HasDiscount)]
	public bool? HasDiscount { get; set; }
	
	[DisplayName(Messages.Available)]
	public bool? Available { get; set; }
	
	[DisplayName(Messages.Brand)]
	public int? ProductBrandId { get; set; }
	
	[DisplayName(Messages.Category)]
	public int? ProductCategoryId { get; set; }
}

public class ProductInput : Models.Product
{
	[DisplayName(Messages.Image)]
	public IFormFile? ImageFile { get; set; }

	[DisplayName(Messages.Thumbnail)]
	public IFormFile? ThumbnailFile { get; set; }
}
